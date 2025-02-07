import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface FormState {
    name: string;
    email: string;
    phone: string;
}

const initialState: FormState = {
    name: '',
    email: '',
    phone: '',
};

const formSlice = createSlice({
    name: 'form',
    initialState,
    reducers: {
        submitForm: (_state, action: PayloadAction<FormState>) => {
            return action.payload;
        },
    },
});

export const { submitForm } = formSlice.actions;
export default formSlice.reducer;