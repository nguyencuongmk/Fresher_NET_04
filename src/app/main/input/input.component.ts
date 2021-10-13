import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { task } from 'src/app/models/model';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.css']
})
export class InputComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  // Properties
  @Output() addNewTaskEvent = new EventEmitter<task>();
  newTask: task = { id: 0, name: '', isCompleted: false };

  // Functions
  addNewTask() {
    return this.addNewTaskEvent.emit(this.newTask);
  }
}
