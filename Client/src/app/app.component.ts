import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HelperForms } from './forms/helper.forms';
import { Book } from './models/book';
import { BookService } from './services/books-service';

interface BookFormValue {
  title: string;
  author: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  books: Book[] = [];

  bookForm = new FormGroup({
    title: new FormControl(null, Validators.required),
    author: new FormControl(null, Validators.required)
  })

  constructor(private booksService: BookService) { }

  ngOnInit(): void {
    this.findBooks();
  }

  get stateMatcher() {
    return HelperForms.getStateMatcher();
  }

  createBook(): void {
    if (this.bookForm.invalid) return;
    const book = this.getBookModel();
    this.booksService.insert(book)
      .subscribe(() => this.findBooks(),
        error => this.handleError(error));

  }

  private findBooks(): void {
    this.booksService.getAll()
      .subscribe(books => this.books = books,
        error => this.handleError(error));
  }

  private handleError(error: unknown): void {
    console.error(error);
  }

  private getBookModel(): Book {
    const result = new Book();
    const form = this.bookForm.value as BookFormValue;
    result.title = form.title;
    result.author = form.author;
    return result;
  }

}
