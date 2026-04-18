import axios from 'axios';
import {TOKEN_KEY} from './Auth';

export const POLICY_SEARCH_HTTP = axios.create({
    baseURL: (process.env.VUE_APP_POLICY_SEARCH_URL ? process.env.VUE_APP_POLICY_SEARCH_URL : "http://localhost:5065/api/")
});

POLICY_SEARCH_HTTP.interceptors.request.use(
    (request) => {
        request.headers.Authorization = 'Bearer ' + localStorage.getItem(TOKEN_KEY);
        return request;
    },
    (error) => Promise.reject(error)
);
