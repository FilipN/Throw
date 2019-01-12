import React, { Component } from 'react';
import { Col, Grid, Row, Button, Modal , FormControl} from 'react-bootstrap';
import './NavMenu.css';
import axios from 'axios';

export class NavMenu extends Component {
    displayName = NavMenu.name

    constructor(props, context) {
        super(props, context);

        this.handleShow = this.handleShow.bind(this);
        this.handleClose = this.handleClose.bind(this);
        this.handleModalOpen = this.handleModalOpen.bind(this);
        this.handleChange = this.handleChange.bind(this);


        this.state = {
            show: false
        };
    }

    handleClose() {
        this.setState({ show: false });
        var key = this.state.value;
        var par = {
            "name":key
        };

        axios({
            method: 'post',
            url: 'api/Project/Create',
            data: par
        }).then(res => {
            console.log("Success");
            window.location.href = 'http://localhost:51529/editor/' + res.data.link;

        })


    }

    handleShow() {
        this.setState({ show: true });
    }


    handleModalOpen() {
        //window.location.href = 'http://localhost:51529/editor';
        this.setState({ show: true });
    }


    handleChange(e) {
        this.setState({ value: e.target.value });
    }

    render() {
        return (
            <div>
            <Grid fluid >
                <Row>
                        <Col lg={4} className="nav-button" onClick={this.handleModalOpen}> New Project</Col>
                    <Col lg={4} className="nav-button"> Load Project</Col>
                    <Col lg={4} className="nav-button"> About  </Col>
                </Row>
                <Row>
                </Row>
            </Grid>
                <Modal show={this.state.show} onHide={this.handleClose} animation={false} >
                <Modal.Header closeButton>
                    <Modal.Title>New Project Name</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                        <FormControl
                            type="text"
                            placeholder="Throw project"
                            onChange={this.handleChange}
                            style={{ margin: 'auto', width: '80%' }}
                        />
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={this.handleClose}>Close</Button>
                </Modal.Footer>
             </Modal>
                </div>
        );
    }
}
