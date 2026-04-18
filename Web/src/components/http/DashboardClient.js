import axios from 'axios';
import {TOKEN_KEY} from './Auth';

export const DASHBOARD_HTTP = axios.create({
    baseURL: (process.env.VUE_APP_DASHBOARD_URL ? process.env.VUE_APP_DASHBOARD_URL : "http://localhost:5035/api/")
});

DASHBOARD_HTTP.interceptors.request.use(
    (request) => {
        request.headers.Authorization = 'Bearer ' + localStorage.getItem(TOKEN_KEY);
        return request;
    },
    (error) => Promise.reject(error)
);
