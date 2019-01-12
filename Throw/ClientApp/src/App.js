import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Login } from './components/Login';
import { Editor } from './components/Editor';



export default class App extends Component {
  displayName = App.name

  render() {
    return (
        <Layout>
            <Route path='/editor' component={Editor} />
            <Route exact path='/login' component={Login} />
            <Route exact path='/' component={Login} />
            <Route exact path='/home' component={Home} />
            <Route path='/counter' component={Counter} />
            <Route path='/fetchdata' component={FetchData} />
      </Layout>
    );
  }
}
