import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class SocialloginService {
  url;
  constructor(private http: HttpClient) { }

  Savesresponse(response) {
    this.url = 'https://localhost:44369/Api/user/login';
    return this.http.post(this.url, response);
  }

}
