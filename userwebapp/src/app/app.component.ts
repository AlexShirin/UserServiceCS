import {TemplateRef, ViewChild} from '@angular/core';
import {Component, OnInit} from '@angular/core';
import {User} from './user';
import {UserService} from './user.service';

@Component({
    selector: 'my-app',
    templateUrl: './app.component.html',
    providers: [UserService]
})
export class AppComponent implements OnInit {
    //типы шаблонов
    @ViewChild('readOnlyTemplate', {static: false}) readOnlyTemplate: TemplateRef<any>|undefined;
    @ViewChild('editTemplate', {static: false}) editTemplate: TemplateRef<any>|undefined;

    editedUser: User|null = null;
    userList: Array<User>;
    isNewRecord: boolean = false;
    statusMessage: string = "";

    constructor(private service: UserService) {
        this.userList = new Array<User>();
    }

    ngOnInit() {
        this.loadUsers();
    }

    //загрузка пользователей
    private loadUsers() {
        this.service.getUsers().subscribe((data: Array<User>) => {
            this.userList = data;
        });
    }
    // добавление пользователя
    addUser() {
        this.editedUser = new User(0,"",0);
        this.userList.push(this.editedUser);
        this.isNewRecord = true;
    }

    // редактирование пользователя
    editUser(user: User) {
        this.editedUser = new User(user._id, user.name, user.age);
    }
    // загружаем один из двух шаблонов
    loadTemplate(user: User) {
        if (this.editedUser && this.editedUser._id === user._id) {
            return this.editTemplate;
        } else {
            return this.readOnlyTemplate;
        }
    }
    // сохраняем пользователя
    saveUser() {
        if (this.isNewRecord) {
            // добавляем пользователя
            this.service.createUser(this.editedUser as User).subscribe(data => {
                this.statusMessage = 'Данные успешно добавлены',
                    this.loadUsers();
            });
            this.isNewRecord = false;
            this.editedUser = null;
        } else {
            // изменяем пользователя
            this.service.updateUser(this.editedUser as User).subscribe(data => {
                this.statusMessage = 'Данные успешно обновлены',
                    this.loadUsers();
            });
            this.editedUser = null;
        }
    }
    // отмена редактирования
    cancel() {
        // если отмена при добавлении, удаляем последнюю запись
        if (this.isNewRecord) {
            this.userList.pop();
            this.isNewRecord = false;
        }
        this.editedUser = null;
    }
    // удаление пользователя
    deleteUser(user: User) {
        this.service.deleteUser(String(user._id)).subscribe(data => {
            this.statusMessage = 'Данные успешно удалены',
                this.loadUsers();
        });
    }
}