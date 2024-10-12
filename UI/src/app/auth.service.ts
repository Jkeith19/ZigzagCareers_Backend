import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../environments/environment.development';

interface TokenResponse {
  access_token: string;
  expires_in: number;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private accessToken: string | null = null;

  constructor(private http: HttpClient) {}

  getAccessToken(): Observable<TokenResponse> {
    const body = new URLSearchParams({
      grant_type: 'client_credentials',
      client_id: environment.clientId,
      client_secret: environment.clientSecret,
    }).toString();
  
    return this.http.post<TokenResponse>(environment.tokenUrl, body, {
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    }).pipe(
      tap(response => {
        console.log('Access token retrieved:', response.access_token);
        this.accessToken = response.access_token;
      })
    );
  }

  get token(): string | null {
    return this.accessToken;
  }
}