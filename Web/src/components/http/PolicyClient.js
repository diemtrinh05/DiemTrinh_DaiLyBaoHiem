import axios from 'axios';
import {TOKEN_KEY} from './Auth';

export const POLICY_HTTP = axios.create({
    baseURL: (process.env.VUE_APP_POLICY_URL ? process.env.VUE_APP_POLICY_URL : "http://localhost:5050/api/")
});

POLICY_HTTP.interceptors.request.use(
    (request) => {
        request.headers.Authorization = 'Bearer ' + localStorage.getItem(TOKEN_KEY);
        return request;
    },
    (error) => Promise.reject(error)
);
