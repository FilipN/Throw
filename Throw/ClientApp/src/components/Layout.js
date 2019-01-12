import React, { Component } from 'react';
import { Col, Grid, Row, Panel} from 'react-bootstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  displayName = Layout.name

    render() {


        return (

            <Panel> {this.props.children} </Panel>
      /*<Grid fluid>
        <Row>
 
          <Col sm={9}>
            
          </Col>
        </Row>
      </Grid>*/
    );
  }
}
