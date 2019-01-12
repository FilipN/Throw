import React, { Component } from 'react';
import { Col, Grid, Row, Button, Alert } from 'react-bootstrap';
import './Editor.css';
import { NavMenu } from "./NavMenu";


export class Editor extends Component {
    displayName = Editor.name

  render() {
      return (
          <div>
         <NavMenu> </NavMenu>
          <Grid fluid>
              <Row> </Row>
              <Row > </Row>
              <Row > </Row>
              </Grid>
           </div>
    );
  }
}
