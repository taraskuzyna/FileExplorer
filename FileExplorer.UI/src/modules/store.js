import { createStore, combineReducers } from 'redux';
import fileSystem from '../reducers/fileSystemReducer';

const store = createStore(combineReducers({ fileSystem }),
  {},
  typeof window !== 'undefined' ? window.devToolsExtension && window.devToolsExtension() : undefined);

export default store;