import { Component } from '@angular/core';
import { task } from './models/model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // Pass data to "list"
  tasks: task[] = [
    { id: 1, name: 'Todo 1', isCompleted: false },
    { id: 2, name: 'Todo 2', isCompleted: true }
  ];


  // Function
  removeTask(task: task) {
    const index = this.tasks.findIndex(x => x.id === task.id);
    this.tasks.splice(index, 1);
  }

  changeStatus(task: task) {
    const index = this.tasks.findIndex(x => x.id === task.id);
    this.tasks[index].isCompleted = !this.tasks[index].isCompleted;
  }

  addNewTask(task:task){
    if (task.name.length == 0) {
      alert('Please enter a new task');
    }
    else {
      this.tasks.push({
        id: this.tasks.length + 1,
        name: task.name,
        isCompleted: false
      });
      task.name = '';
    }
  }
}
