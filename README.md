# FrameForge

Project Structure:
# FrameFroge:
1. Основний ASP.NET проект:
  1) Папка Views - Всі cshtml сторінки(формат: html + c#)
  2) Папка wwwroot - Всі статичні файли(css, js, фото)
  3) Controllers - Обробнкики запитів які повертають відповідні сторінки
2. Entities - Усі класи моделі(користувач, адмін, задачі і т.д.)
3. Services - Уся бізнес-логіка
4. ServiceContracts - Інтерфейси для усіх сервісів(так модно)
5. ServiceTests - xUnit тести сервісів(щоб писати і тестувати сервіси окремо від фронтенду і контролерів)
