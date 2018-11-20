import React, { Component } from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Paper, List, ListItem, ListItemText, ListItemIcon } from '@material-ui/core';
import Folder from '@material-ui/icons/Folder';
import InsertDriveFile from '@material-ui/icons/InsertDriveFile';
import _ from 'lodash'

const styles = theme => ({
  main: {
    width: '50%',
    margin: 10,
    overflowY: 'auto'
  },
  header: {
    backgroundColor: 'lightgrey',
    height: 40
  }
});

class DirectoryExplorer extends Component {

  getCurrentDirectory = (fileSystem, currentPath) => {
    let result = fileSystem;
    for (let i = 0; i < currentPath.length; i++) {
      result = _(result.directories).find(x => x.name === currentPath[i]);
    }
    return result;
  }

  render() {
    const { classes, fileSystem, currentPath } = this.props;
    const currentDirectory = this.getCurrentDirectory(fileSystem, currentPath)
    console.log(currentPath);
    const parentFolder = currentPath.length === 0 ? [] : [(
      <ListItem button onClick={this.props.onExploreParent}>
        <ListItemIcon>
          <Folder />
        </ListItemIcon>
        <ListItemText primary='..' />
      </ListItem>
    )]
    const directoryItems = currentDirectory.directories.map(x => (
      <ListItem button onClick={()=> this.props.onExploreFolder(x.name)}>
        <ListItemIcon>
          <Folder />
        </ListItemIcon>
        <ListItemText primary={x.name} />
      </ListItem>
    ))
    const filesItems = currentDirectory.files.map(x => (
      <ListItem button>
        <ListItemIcon>
          <InsertDriveFile />
        </ListItemIcon>
        <ListItemText primary={x.name} />
      </ListItem>
    ))
    return (
      <Paper className={classes.main}>
        <div className={classes.header}></div>
        <List component="nav">
          {parentFolder.concat(directoryItems).concat(filesItems)}
        </List>
      </Paper>
    );
  }
}

export default withStyles(styles)(DirectoryExplorer)
