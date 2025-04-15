# LibraryModsen

Тестовое задание для стажировки по направлению .NET

### Установка

  1. Копировать репозиторий
     
      ```bash
      git clone https://github.com/TheSnoopcheg/LibraryModsen
      ```

  2. Установить MS SQL Server
  
      ```bash
      docker pull mcr.microsoft.com/mssql/server:2022-latest
      ```
      
  3. Запустить SQL Server

      ```bash
      docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=testTEST1" -p 1433:1433 --name modsenlib --hostname modsenlib -d mcr.microsoft.com/mssql/server:2022-latest
      ```

  4. Создать начальные миграции и собрать проект

      Выполнять из директории с "LibraryModsen.sln":
      ```bash
      dotnet ef migration add initial -s LibraryModsen -p LibraryModsen.Persistence
      docker build -f LibraryModsen\Dockerfile --force-rm -t test/try .
      ```

  5. Запуск

      Создание таблиц в БД, инициализация учётной записи админа:
      ```bash
      docker run --name=library -p 32000:8080 -it test/try /seed
      docker container rm library
      ```
      Запуск Web API
      ```bash
      docker run --name=library -p 32000:8080 -it test/try
      ```
