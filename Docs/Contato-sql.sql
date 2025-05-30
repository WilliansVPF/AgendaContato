create database AgendaContato;
use AgendaContato;

create table Usuario(
idUsuario int primary key not null auto_increment,
nome varchar(45) not null,
email varchar(45) not null unique,
senha varchar(60) not null,
salt varchar(29) not null
);

create table Contato(
idContato int primary key not null auto_increment,
nome varchar(45) not null,
sobrenome varchar(40) not null,
idUsuario int not null,
foreign key (idUsuario) references Usuario (idUsuario)
);

create table TipoContato(
idTipoContato int primary key not null auto_increment,
tipo varchar(45) not null unique
);

create table EnderecoContato(
idEnderecoContato int primary key not null auto_increment,
valor varchar(45) not null,
observacao varchar(45),
idTipoContato int not null,
idContato int not null,
foreign key (idTipoContato) references TipoContato (idTipoContato),
foreign key (idContato) references Contato (idContato) on delete cascade
);

insert into TipoContato values(0, "Telefone");
insert into TipoContato values(0, "Fax");
insert into TipoContato values(0, "Email");
insert into TipoContato values(0, "Site");