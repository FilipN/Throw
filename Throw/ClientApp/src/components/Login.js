import React, { Component } from 'react';
import FacebookLogin from 'react-facebook-login';
import GoogleLogin from 'react-google-login';
import axios from 'axios';
import { Col, Grid, Row, Button, Alert } from 'react-bootstrap';
import './Login.css';

const responseFacebook = (response) => {
    console.log("Uspeh");
    axios({
        method: 'post',
        url: 'api/User/Login',
        data: response
    }).then(res => {
        console.log("Success");
        window.location.href = 'http://localhost:51529/home';

    })
    console.log(response);
}

const responseGoogle = (response) => {
    console.log(response);
}

export class Login extends Component {

    incrementCounter() {
        /*this.setState({
            currentCount: this.state.currentCount + 1
        });*/
    }
  render() {
    return (
        <Grid>
            <Row style={{height:300}}> </Row>
            <Row className="justify-content-md-center" style={{marginBottom:100}}>
                <Col md> <h1>Welcome to Collaborato.rs</h1> </Col>
            </Row>
            <Row className="justify-content-md-center" >

                <Col md style={{marginRight:30}}>
                    <Button className="aboutus-button">About us</Button>
                </Col>

                <Col md>
                    <FacebookLogin
                        appId="1488404707928531"
                        fields="name,email,picture"
                        cssClass="facebook-button"
                        callback={responseFacebook} />
                </Col>
                <Col md style={{ marginLeft: 30 }}>
                    <GoogleLogin
                        clientId="1013793838171-du1855fdm66255s4mq96vh1le8mujjfs.apps.googleusercontent.com"
                        buttonText="Google login"
                        className="google-button"
                        onSuccess={responseGoogle}
                        onFailure={responseGoogle}
                    />
                </Col>
            </Row>
        </Grid>
    );
  }
}
