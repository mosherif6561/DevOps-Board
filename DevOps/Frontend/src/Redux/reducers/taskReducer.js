import { createSlice } from "@reduxjs/toolkit";

const taskSlice = createSlice({
  name: "tasks",
  initialState: {
    taskData: [],
    searchValue: [""],
  },
  reducers: {
    setTasksData: (state, action) => {
      state.taskData = action.payload;
    },
    setSearchValue: (state, action) => {
      state.searchValue = [""];
      state.searchValue = [action.payload];
    },
    setArraySearchValue: (state, action) => {
      state.searchValue = [""];
      state.searchValue = action.payload;
    },
  },
});

export const { setTasksData, setSearchValue, setArraySearchValue } = taskSlice.actions;
export default taskSlice.reducer;
