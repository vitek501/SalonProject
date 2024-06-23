# SalonProject

Проект для управления салонами с расчетом стоимости услуг с учетом скидок. Использует SQLite для хранения данных и NUnit для тестирования.

## Оглавление

- [Требования](#requirements)
- [Установка](#install)
- [Использование](#usage)
- [Структура проекта](#project-structure)

## <a id="requirements">Требования</a> 

- .NET SDK 6.0 или выше
- SQLite
- NUnit

## <a id="install">Установка</a>

 Клонируйте репозиторий:

    ```bash
    git clone https://github.com/vitek501/SalonProject.git
    cd SalonProject
    ```
Выберите Presentation.ConsoleApp в качестве запускаемого.

## <a id="usage">Использование</a>

Проект включает функционал для расчета стоимости услуг в салонах с учетом скидок. Данные о салонах хранятся в базе данных SQLite.

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

