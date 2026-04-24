# WIGGLER Website

This folder contains the public-facing website for WIGGLER by SMBA.

## Purpose

The website is the brand, product story, and browser demo.

The desktop app remains the actual Windows utility that can monitor idle time and move the system mouse.

## Local development

```powershell
npm install
npm run dev
```

## Production build

```powershell
npm run build
```

## Notes

- The site includes a visual cursor-motion demo in the browser.
- Browsers cannot control the real system mouse, so the website does not replace the desktop app.
- The desktop project lives in `../desktop/WigglerBySmba`.
