
	--PostgreSQL--
 
 --1. Постоянные таблицы и связи между ними, количество таблиц и наличие связей должно соответствовать заданию, 
 --допускается увеличение числа таблиц и их полей для более адекватного представления предметной области.

create table Groups
( 
    Id serial primary key,
    name varchar(20) not null unique,
    specialty varchar(100) not null,
    number_of_students int not null,
    semester int not null
);

create table Classroom
(
    Id serial primary key,
    name varchar(50) not null unique,
    capacity int not null
);

create table Subject
(
    Id serial primary key,
    name varchar(100) not null unique
);

create table Teacher
(
    Id serial primary key,
    full_name varchar(100) not null,
    subject_id integer references Subject(Id) 
);

create table Lesson
(
    Id serial primary key,
    date_lesson date not null,
    lesson_number int not null,
    group_id integer references Groups(Id) not null,
    classroom_id integer references Classroom(Id) not null,
    teacher_id integer references Teacher(Id) not null
);

alter table Subject 
add lesson_id integer references Lesson(Id) default null

  --2. В приложении реализовать не менее пяти запросов, включая (для демонстрации навыков работы)
  --a. Составной многотабличный запрос с параметром, включающий соединение таблиц и CASE-выражение;
 
select Lesson.Id, Lesson.date_lesson, Lesson.lesson_number, Groups.name as group_name, Classroom.name as classsroom_name,
       (case when Groups.number_of_students > Classroom.capacity then 'students will not fit in the classroom'
        else 'students will fit in the classroom' end) as capacity_check    
from Lesson
     join Groups on Lesson.group_id = Groups.Id
     join Classroom on Lesson.classroom_id = Classroom.Id
	
 --b. На основе обновляющего представления (многотабличного VIEW), в котором критерий упорядоченности задает пользователь при выполнении;
create or replace view lesson_veiw as 
    select Lesson.Id, Lesson.date_lesson, Lesson.lesson_number, Groups.name as group_name, 
           Groups.specialty, Classroom.name as classroom_name, Teacher.full_name, Subject.name as subject_name
from Lesson
      join Groups on Lesson.group_id = Groups.Id
      join Classroom on Lesson.classroom_id = Classroom.Id
      join Teacher on Lesson.teacher_id = Teacher.Id
      join Subject on Teacher.subject_id = Subject.Id
order by Lesson.date_lesson

 --c. Запрос, содержащий коррелированные и некоррелированные подзапросы в разделах SELECT, FROM и WHERE (в каждом хотя бы по одному);
select Lesson.Id, Lesson.date_lesson, Lesson.lesson_number,
        (select Groups.name from Groups
            where Groups.Id = group_id) as group_name,
        (select Classroom.name from Classroom 
            where Classroom.Id = classroom_id) as classroom_name,
        (select Teacher.full_name from Teacher 
            where Teacher.Id = teacher_id) as teacher_name
from (select * from Lesson 
        where lesson_number < 4) as Lesson
where (select Classroom.capacity from Classroom
            where Classroom.Id = classroom_id) > 
       (select Groups.number_of_students from Groups 
            where Groups.Id = group_id)

 --d. Многотабличный запрос, содержащий группировку записей, агрегативные функции и параметр, используемый в разделе HAVING;
select Lesson.Id, Lesson.date_lesson, Lesson.lesson_number, Groups.name as group_name,
       min(Groups.number_of_students) as number_of_students
from Lesson, Groups
    where Groups.Id = Lesson.group_id
group by Lesson.Id, Groups.name
    having (min(Groups.number_of_students) < 26);

 --e. Запрос, содержащий предикат ANY(SOME) или ALL;
select Lesson.Id, Lesson.date_lesson, Lesson.lesson_number, Classroom.name as classroom_name
from Lesson
     join Classroom on Lesson.classroom_id = Classroom.Id
     where Lesson.classroom_id = any 
     (select Classroom.Id from Classroom where Classroom.name = '321b')

 --3. Создать индексы для увеличения скорости выполнения запросов.
create index i_group_name on Groups using btree(name);
create index i_classroom_name on Classroom using btree(name);
create index i_Teacher_full_name on Teacher using btree(full_name);
create index i_subject_name on Subject using btree(name);

 --4. В таблице (в соответствии с вариантом) предусмотреть поле, которое заполняется автоматически по срабатыванию триггера
 -- при добавлении, обновлении и удалении данных, иметь возможность продемонстрировать работу триггера при работе приложения.
 -- Триггеры должны обрабатывать только те записи, которые были добавлены, изменены или удалены в ходе текущей операции (транзакции).

create table History
(
    Id serial primary key,
    group_id int not null,
    operation varchar(100) not null,
    group_name varchar(100) not null,
    create_at date not null default current_date
);

 -- добавление новой группы 

create or replace function Groups_add_history()
returns trigger
language plpgsql
as $function$
begin
    insert into History(group_id, operation, group_name)
    values (new.Id, 'add group ',new.name);
    return new;
end;
$function$;

create trigger tr_group_add
after insert on Groups
for each row execute procedure Groups_add_history()

 -- удаление группы 

create or replace function Groups_deletion_history()
returns trigger
language plpgsql
as $function$
begin
    insert into History(group_id, operation, group_name)
    values (old.Id, 'deleting a group ' , old.name);
    return old;
end;
$function$;

create trigger tr_group_deletion
after delete on Groups
for each row execute procedure Groups_deletion_history()

 -- изменение группы 

create or replace function Groups_update_history()
returns trigger
language plpgsql
as $function$
begin
    insert into History(group_id, operation, group_name)
    values (new.Id, 'group update ' , new.name);
    return new;
end;
$function$;

create trigger tr_group_update
after update on Groups
for each row execute procedure Groups_update_history()

 --5.Операции добавления, удаления и обновления реализовать в виде хранимых процедур (с параметрами) хотя бы для одной таблицы; 
 -- для остальных допустимо использовать возможности связывания полей ввода в приложении с полями БД.


 -- добавление группы 
create or replace function add_group(_name text, _specialty text, _number_of_students int, _semester int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    insert into Groups(name, specialty, number_of_students, semester)
                values(_name, _specialty, _number_of_students, _semester);
    return checker;
end;
$$;
    
 -- изменение группы 
create or replace function update_group(_Id int, _name text, _specialty text, _number_of_students int, _semester int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    update Groups
        set name = _name,
            specialty = _specialty,
            number_of_students = _number_of_students,
            semester = _semester
        where Id = _Id;
    return checker;
end;
$$; 

 -- удаление группы 
create or replace function delete_group(_Id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    delete from Groups
        where Id = _Id;
    return checker;
end;
$$; 

 -- добавление аудитории 
create or replace function add_classroom(_name text, _capacity int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    insert into Classroom(name, capacity)
                values(_name, _capacity);
    return checker;
end;
$$; 

 -- изменение аудитории 
create or replace function update_classroom(_Id int, _name text, _capacity int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    update Classroom
        set name = _name,
            capacity = _capacity
        where Id = _Id;
    return checker;
end;
$$; 

 -- удаление аудитории 
create or replace function delete_classroom(_Id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    delete from Classroom
        where Id = _Id;
    return checker;
end;
$$; 

 -- добавление дисциплины 
create or replace function add_subject(_name text, _lesson_id int default null)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    insert into Subject(name, lesson_id)
                values(_name, _lesson_id);
    return checker;
end;
$$; 

 -- изменение дисциплины 
create or replace function update_subject(_Id int, _name text, _lesson_id int default null)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    update Subject
        set name = _name,
            lesson_id = _lesson_id
        where Id = _Id;
    return checker;
end;
$$; 

select update_subject(2,'Линейная алгебра')

 -- удаление дисциплины 
create or replace function delete_subject(_Id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true; 
begin
    delete from Subject
        where Subject.Id = _Id;
    return checker;
end;
$$; 

 -- добавление преподователя 
create or replace function add_teacher(_full_name text, _subject_id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    insert into Teacher(full_name, subject_id)
                values(_full_name, _subject_id);
    return checker;
end;
$$; 

 -- изменение преподователя 
create or replace function update_teacher(_Id int, _full_name text, _subject_id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    update Teacher
        set full_name = _full_name,
            subject_id = _subject_id
        where Id = _Id;
    return checker;
end;
$$; 

 -- удаление преподователя 
create or replace function delete_teacher(_Id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    delete from Teacher
        where Id = _Id;
    return checker;
end;
$$; 

 -- добавление пары 
create or replace function add_Lesson(_date_lesson date, _lesson_number int, _group_id int, _classroom_id int, _teacher_id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    if (_lesson_number > 10 or _lesson_number < 1) then
    raise exception 'неверно указан номер пары';
    checker := false;
    end if;
    if(checker = true) then
    insert into Lesson(date_lesson, lesson_number, group_id, classroom_id ,teacher_id)
                values(_date_lesson, _lesson_number, _group_id, _classroom_id, _teacher_id);
    end if;
    return checker;
end;
$$;

 -- изменение пары 
create or replace function update_Lesson(_Id int, _date_lesson date, _lesson_number int, _group_id int, _classroom_id int, _teacher_id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    if (_lesson_number > 10 or _lesson_number < 1) then
    raise exception 'неверно указан номер пары';
    checker := false;
    end if;
    if(checker = true) then
    update Lesson
        set date_lesson = _date_lesson,
            lesson_number = _lesson_number,
            group_id = _group_id,
            classroom_id = _classroom_id,
            teacher_id = _teacher_id
        where Id = _Id;
    end if;
    return checker;
end;
$$;

 -- удаление пары 
create or replace function delete_Lesson(_Id int)
returns boolean 
language plpgsql
as $$
declare
checker boolean:= true;
begin
    delete from Lesson
        where Lesson.Id = _Id;
    return checker;
end;
$$; 

 --6. Реализовать отдельную хранимую процедуру, состоящую из нескольких отдельных операций в виде единой транзакции, 
 -- которая при определенных условиях может быть зафиксирована или откатана.

create or replace procedure next_year_groups(group_id int)
language plpgsql
as $$
declare
    current_year int;
begin
    select Groups.year into current_year from Groups where Groups.Id = group_id;
    if current_year < 5  then
            update Groups
            set Groups.year = Groups.yaer + 1 where Groups.Id = group_id;
    else
            delete from Groups 
                where Groups.Id = group_id;
    end if;
end; $$
 --7. В триггере или хранимой процедуре реализовать курсор на обновления отдельных данных.
		
create or replace function group_update(
    _id int, 
    _name text,
    _specialty text,
    _number_of_students int,
    semester int
)
returns boolean
language plpgsql
as $$
declare
checker boolean := true;
_cursor cursor (cursor_id bigint) for select * from Groups where Groups.Id = cursor_id;
begin
    open _cursor(cursor_id := _id);
    move first from _cursor;
    update Groups 
        set 
            Groups.name = _name,
            Groups.specialty = _specialty,
            Groups.number_of_students = _number_of_students,
            Groups.semester = _semestr
    where current of _cursor;
    close _cursor;
return checker;
end;
$$;

 --8. В запросе (из пункта 2 или в дополнительном к тому перечню) использовать собственную скалярную функцию, 
 -- а в хранимой процедуре – векторную (или табличную) функцию. Функции сохранить в базе данных.

 --Скалярная функция
create or replace function get_number_of_students(group_name text)
returns int
language plpgsql
as $$
declare
_number_of_students int;
begin 
    select Groups.number_of_students into _number_of_students from Groups
        where Groups.name = group_name;
    return _number_of_students;
end;
$$;

 --Табличная функция 
create or replace function return_table_group(group_name text)
returns table (_id int, _name text, _specialty text)
language plpgsql
as $$
begin
    select Groups.Id, Groups.name, Groups.specialty from Groups
        where Groups.name = group_name;
end;
$$;
 --9. Распределение прав пользователей: предусмотреть не менее двух пользователей с разным набором привилегий. 
 -- Каждый набор привилегий оформить в виде роли.

create role Timetable_Teacher;
create role Timetable_Student;
create role Timetable_Admin;

grant select, insert on all tables in schema public to Timetable_Teacher;
grant select on all tables in schema public to Timetable_Student;
grant select, update, insert, delete on all tables in schema public to Timetable_Admin;
grant usage, select on all sequences in schema public to Timetable_Admin; 

create user T_Teacher password '12345';
create user T_Student password '12345';
create user T_Admin password 'admin';

grant Timetable_Teacher to T_Teacher;
grant Timetable_Student to T_Student;
grant Timetable_Admin to T_Admin;
