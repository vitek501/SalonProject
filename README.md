# SalonProject

Проект для управления салонами с расчетом стоимости услуг с учетом скидок. Использует SQLite для хранения данных и NUnit для тестирования.

## Оглавление

- [Требования](#requirements)
- [Установка](#install)
- [Использование](#usage)
- [Тестирование](#test)
- [Структура проекта](#project-structure)

## <a id="requirements">Требования</a> 

- .NET SDK 6.0 или выше
- SQLite
- NUnit

## <a id="install">Установка</a>

1. Клонируйте репозиторий:

    ```bash
    git clone https://github.com/vitek501/SalonProject.git
    cd SalonProject
    ```

2. Установите необходимые пакеты:

    ```bash
    dotnet restore
    ```

## <a id="usage">Использование</a>

Проект включает функционал для расчета стоимости услуг в салонах с учетом скидок. Данные о салонах хранятся в базе данных SQLite.

### <a id="run_project_">Запуск проекта</a>

Для запуска проекта используйте команду:

```bash
dotnet run --project SalonProject
```


## <a id="test">Тестирование</a>

```bash
dotnet test
```

## <a id="project-structure">Струкура проекта</a>

```plaintext
SalonProject
│
├── src
│   ├── Core
│   ├── Infrastructure
│   └── Presentation.ConsoleApp
│
└── test
    └── UnitTests
```

