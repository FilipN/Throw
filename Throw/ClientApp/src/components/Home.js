import React, { Component } from 'react';
import { Col, Grid, Row, Button, Alert } from 'react-bootstrap';
import './Home.css';
import { NavMenu } from "./NavMenu";


export class Home extends Component {
  displayName = Home.name

  render() {
      return (
         <NavMenu> </NavMenu>

    );
  }
}
