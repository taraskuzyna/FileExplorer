import React, { Component } from 'react';
import { withStyles } from '@material-ui/core/styles';
import { Paper } from '@material-ui/core';

const styles = theme => ({
  main: {
    width: '50%',
    margin: 10
  },
  header: {
    backgroundColor: 'lightgrey',
    height: 40
  }
});

class PanelContent extends Component {
  render() {
    const { classes } = this.props;

    return (
      <Paper className={classes.main}>
        <div className={classes.header}></div>
      </Paper>
    );
  }
}

export default withStyles(styles)(PanelContent)
