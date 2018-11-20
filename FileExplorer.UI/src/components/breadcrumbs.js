import React, { Component } from 'react';
import { withStyles } from '@material-ui/core/styles';


const styles = theme => ({
  header: {
    margin: 10
  },
});

class Breadcrumbs extends Component {
  render() {
    const { classes, currentPath } = this.props;

    return (
      <h1 className={classes.header}>{`../${currentPath.join('/')}`}</h1>
    );
  }
}

export default withStyles(styles)(Breadcrumbs)
