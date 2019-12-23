import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { baseURL } from '../../baseUrl';
import { JsonPipe } from '@angular/common';

@Injectable()
export default class TaskService {
  private headers: HttpHeaders;

  constructor(private http: HttpClient)
    {
      const token = localStorage.getItem('JWT');
      this.headers = new HttpHeaders({
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      });
    }

    public getItems = () => {
        return this.http.get(`${baseURL}api/task?userid=${localStorage.getItem("userId")}`, {headers: this.headers});
    }

    public sendSize = (size) => {
        return this.http.post(`${baseURL}api/task?matrixSize=${size}&userid=${localStorage.getItem("userId")}`, { headers: this.headers });
    }

    public cancelTask = (taskId) => {
      return this.http.post(`${baseURL}api/task/cancelTask?taskId=${taskId}`, { headers: this.headers }).subscribe();
    }

    public getResult = (taskId) => {
        return this.http.get(`${baseURL}api/task/getTaskResult?taskId=${taskId}`, { headers: this.headers });
    }
};
