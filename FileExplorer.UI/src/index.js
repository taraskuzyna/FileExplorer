import 'babel-polyfill';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import App from './components/app';
import store from './modules/store';

ReactDOM.render(<Provider store={store}><App /></Provider>, document.getElementById('root'));