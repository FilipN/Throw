import React, { Component } from 'react';
import FacebookLogin from 'react-facebook-login';
import GoogleLogin from 'react-google-login';
import axios from 'axios';


const responseFacebook = (response) => {
    console.log("Uspeh");
    axios({
        method: 'post',
        url: 'api/SampleData/Login',
        data: response
    }).then(res => {
        console.log("Success");
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
      <div>
            <h1>Welcome to Collaborato.rs</h1>
            <FacebookLogin
                appId="1488404707928531"
                autoLoad={true}
                fields="name,email,picture"
                callback={responseFacebook} />
            <GoogleLogin
                clientId="1013793838171-du1855fdm66255s4mq96vh1le8mujjfs.apps.googleusercontent.com"
                buttonText="Google login"
                onSuccess={responseGoogle}
                onFailure={responseGoogle}
            />
      </div>
    );
  }
}
