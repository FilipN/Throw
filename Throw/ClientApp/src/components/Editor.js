import React, { Component} from 'react';
import { Col, Grid, Row, Button, Modal, FormControl} from 'react-bootstrap';
import './Editor.css';
import { NavMenu } from "./NavMenu";


export class Editor extends Component {
    displayName = Editor.name

    

  render() {
      return (
          <div>
         <NavMenu> </NavMenu>
          <Grid fluid>
                  <Row style={{ height: 80, backgroundColor: 'gray' }}>
                      <Col lg={2} style={{paddingTop:25,textAlign:'center'}}> Project name: </Col>
                      <Col lg={3} style={{ paddingTop: 20 }}>

                          <FormControl
                              type="text"
                              placeholder="Throw project"
                              onChange={this.handleChange}
                              style={{ margin: 'auto', width: '80%' }}
                          /> </Col>
                      <Col> </Col>
                      <Col> </Col>
                  </Row>

                  <Row style={{ paddingTop: 30, height: 1500, backgroundColor: 'skyblue' }}>
                      <Col lg={12}>
                          <Row >
                              <Col lg={10}>
                                  <FormControl componentClass="textarea" style={{ height: 500 }} />
                              </Col>
                              <Col lg={2}>
                              </Col>

                          </Row>

                          <Row >
                              <Col lg={10}>
                                  <FormControl componentClass="textarea" style={{ height: 200,color:'white',backgroundColor:'black' }} />
                              </Col>
                              <Col lg={2}>
                              </Col>

                          </Row>
                      </Col>
                  </Row>



              <Row > </Row>
              </Grid>

              
           </div>
    );
  }
}
