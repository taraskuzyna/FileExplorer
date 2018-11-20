import store from '../modules/store';

const setFileSystemStructure = (data) =>  store.dispatch({ type: "SET_FILE_SYSTEM_STRUCTURE", data });

export {
  setFileSystemStructure
};

export default {
  setFileSystemStructure,
};
