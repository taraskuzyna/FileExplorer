import React, { Component } from 'react';
import { withStyles } from '@material-ui/core/styles';
import { AppBar, Typography, Toolbar } from '@material-ui/core';
import FileCopy from '@material-ui/icons/FileCopy';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import _ from 'lodash'
import DirectoryExplorer from './directoryExplorer';
import PanelContent from './panelContent';
import Breadcrumbs from './breadcrumbs';
import { setFileSystemStructure } from '../actions/fileSystemActions';

const styles = theme => ({
  root: {
    flexGrow: 1,
    zIndex: 1,
    position: 'relative',
    display: 'flex',
    height: '100%',
  },
  main: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.default,
    padding: theme.spacing.unit * 1,
    minWidth: 0,
  },
  toolbar: theme.mixins.toolbar,
  title: {
    marginLeft: 20,
  },
  content: {
    height: 'calc(100% - 130px)',
    overflowY: 'auto',
    position: 'relative',
    display: 'flex',
  }
});
class Main extends Component {
  constructor(props) {
    super(props)
    this.state = {
      currentPath: [],
      hubConnection: null,
    }
  }

  componentDidMount = () => {
    const hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:4444/file-system')
      .configureLogging(LogLevel.Information)
      .build();

    this.setState({ hubConnection }, () => {
      this.state.hubConnection
        .start()
        .then(() => console.log('Connection started!'))
        .catch(err => console.log('Error while establishing connection :('));

      this.state.hubConnection.on('ReceiveMessage', setFileSystemStructure);
    });
  }

  onExploreFolder = (folder) => {
    let currentPath = this.state.currentPath;
    currentPath = currentPath.concat([folder]);
    this.setState({ currentPath });
  }

  onExploreParent = () => {
    const currentPath = _.clone(this.state.currentPath);
    currentPath.splice(-1, 1)
    this.setState({ currentPath });
  }

  render() {
    const { classes, fileSystem } = this.props;
    const { currentPath } = this.state;
    return (
      <div className={classes.root}>
        <AppBar position="absolute" className={classes.appBar}>
          <Toolbar>
            <FileCopy />
            <Typography variant="title" color="inherit" noWrap className={classes.title}>
              File Explorer
            </Typography>
          </Toolbar>
        </AppBar>

        <main className={classes.main}>
          <div className={classes.toolbar} />
          <Breadcrumbs {...{ currentPath }} />
          <div className={classes.content}>
            <DirectoryExplorer {...{ fileSystem, currentPath, onExploreParent: this.onExploreParent, onExploreFolder: this.onExploreFolder }} />
            <PanelContent />
          </div>
        </main>
      </div>
    );
  }
}

export default withStyles(styles)(Main)
