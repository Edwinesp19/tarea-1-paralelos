// import Vue from 'nativescript-vue'

// import Home from './components/Home'

// new Vue({
//   render: (h) => h('frame', [h(Home)]),
// }).$start()

import Vue from 'nativescript-vue';
import { Application } from '@nativescript/core';
import authModel from './models/authModel';
import Home from './components/Home';
import Login from './components/Login';

new Vue({
  render: (h) => {
    const initialView = authModel.isLoggedIn() ? Home : Login;
    return h('frame', [h(initialView)]);
  }
}).$start();
