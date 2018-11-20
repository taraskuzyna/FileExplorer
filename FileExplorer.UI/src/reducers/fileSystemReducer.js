const initialState = {
  directories: [],
  files: [],
};

export default (state = initialState, action) => {
  switch (action.type) {
    case "SET_FILE_SYSTEM_STRUCTURE":
      return { ...state, ...action.data, };
    default:
      return state;
  }
};
