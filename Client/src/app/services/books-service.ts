import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Book } from "../models/book";

const URL = 'https://localhost:44313/api/Books';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<Book[]> {
    return this.httpClient.get<Book[]>(URL);
  }

  getById(id: string): Observable<Book> {
    return this.httpClient.get<Book>(`${URL}/${id}`);
  }

  insert(args: { title: string, author: string }): Observable<void> {
    const body = JSON.stringify(args)
    return this.httpClient.post<void>(URL, body);
  }

  update(book: Book): Observable<void> {
    const body = JSON.stringify(book)
    return this.httpClient.put<void>(`${URL}/${book.id}`, body);
  }

  delete(id: string): Observable<void> {
    return this.httpClient.delete<void>(`${URL}/${id}`);
  }

}
