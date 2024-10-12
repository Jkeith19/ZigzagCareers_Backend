import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { environment } from '../environments/environment.development';
import { Observable, switchMap, tap } from 'rxjs';
import { Book } from './models/book.model';
import { AsyncPipe } from '@angular/common';
import { AuthService } from './auth.service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, AsyncPipe, FormsModule, ReactiveFormsModule],
  templateUrl: './app.component.html',
  styles: [],
})
export class AppComponent {
  applicationTitle = 'Library Details Register';
  http = inject(HttpClient);
  authService = inject(AuthService);

  booksForm = new FormGroup({
    title: new FormControl<string>(''),
    author: new FormControl<string>(''),
    isbn: new FormControl<string>(''),
    publishedDate: new FormControl<Date | null>(new Date())
  });

  baseUrl: string = environment.baseUrl;
  
  books$ = this.getBooks();

  onFormSubmit() {
    const addBookRequest = {
      title: this.booksForm.value.title,
      author: this.booksForm.value.author,
      isbn: this.booksForm.value.isbn,
      publishedDate: this.booksForm.value.publishedDate
    };

    this.authService.getAccessToken().pipe(
      switchMap(() => {
        const headers = {
          Authorization: `Bearer ${this.authService.token}`,
        };
        return this.http.post(this.baseUrl + 'book', addBookRequest, { headers });
      })
    ).subscribe({
      next: (value) => {
        console.log(value);
        this.books$ = this.getBooks();
        this.booksForm.reset();
      },
      error: (err) => {
        console.error('Error adding book:', err);
      }
    });
  }

  private getBooks(): Observable<Book[]> {
    return this.authService.getAccessToken().pipe(
      switchMap(() => {
        const headers = {
          Authorization: `Bearer ${this.authService.token}`,
        };
        return this.http.get<Book[]>(this.baseUrl + 'books', { headers }).pipe(
          tap(books => {
            console.log('Books retrieved:', books);
          })
        );
      })
    );
  }
}