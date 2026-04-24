import { useEffect, useState } from 'react'
import './App.css'

const patterns = ['Circle', 'Square', 'Triangle', 'Figure 8', 'Parallelogram', 'Random']

const patternConfig = {
  Circle: {
    outline: <circle className="path-stroke" cx="320" cy="196" r="94" />,
    motionPath: 'M 226 196 a 94 94 0 1 1 188 0 a 94 94 0 1 1 -188 0',
    duration: 7.5,
  },
  Square: {
    outline: <path className="path-stroke" d="M 218 104 L 422 104 L 422 308 L 218 308 Z" />,
    motionPath: 'M 218 104 L 422 104 L 422 308 L 218 308 Z',
    duration: 8.5,
  },
  Triangle: {
    outline: <path className="path-stroke" d="M 320 92 L 454 310 L 186 310 Z" />,
    motionPath: 'M 320 92 L 454 310 L 186 310 Z',
    duration: 7.8,
  },
  'Figure 8': {
    outline: (
      <path
        className="path-stroke"
        d="M 198 194
           C 198 116, 308 118, 320 194
           C 332 270, 442 272, 442 194
           C 442 116, 332 118, 320 194
           C 308 270, 198 272, 198 194 Z"
      />
    ),
    motionPath:
      'M 198 194 C 198 116, 308 118, 320 194 C 332 270, 442 272, 442 194 C 442 116, 332 118, 320 194 C 308 270, 198 272, 198 194 Z',
    duration: 9.2,
  },
  Parallelogram: {
    outline: <path className="path-stroke" d="M 234 108 L 448 108 L 394 302 L 180 302 Z" />,
    motionPath: 'M 234 108 L 448 108 L 394 302 L 180 302 Z',
    duration: 8.1,
  },
  Random: {
    outline: (
      <path
        className="path-stroke random-outline"
        d="M 132 286
           L 222 142
           L 338 250
           L 470 118
           L 528 272
           L 384 322
           L 254 194
           L 164 302"
      />
    ),
    motionPath: 'M 132 286 L 222 142 L 338 250 L 470 118 L 528 272 L 384 322 L 254 194 L 164 302',
    duration: 8.8,
  },
}

const copy = {
  en: {
    navFeatures: 'Features',
    navDemo: 'Demo',
    navSettings: 'Settings',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    heroEyebrow: 'Desktop utility, now with a home on the web',
    heroTitle: 'Keep the session alive without fighting the user.',
    heroBody:
      'WIGGLER by SMBA is a lightweight Windows app that waits for idle time, then moves the cursor in clean, bounded shapes until you touch the mouse again.',
    watchDemo: 'Watch the demo',
    desktopDownloads: 'Desktop downloads',
    bestFor: 'Best for',
    bestForValue: 'Windows desktop workflows',
    corePromise: 'Core promise',
    corePromiseValue: 'Idle first. Instant handoff.',
    status: 'Status',
    armed: 'Armed',
    idleDelay: 'Idle delay',
    pattern: 'Pattern',
    tray: 'Tray',
    ready: 'Ready',
    surfaceFootOne: 'The desktop app takes over only after inactivity.',
    surfaceFootTwo: 'Your mouse wins instantly.',
    featureEyebrow: 'Feature',
    features: [
      {
        title: 'Idle-first control',
        body: 'WIGGLER stays armed in the background and only starts moving after you have really been idle.',
      },
      {
        title: 'Instant handoff',
        body: 'The second you touch the mouse, the desktop app stops and gives control back immediately.',
      },
      {
        title: 'Bounded motion',
        body: 'Patterns stay visible, on-screen, and intentional instead of drifting off the edge.',
      },
    ],
    motionEyebrow: 'Motion library',
    motionTitle: 'Visible patterns, quiet behavior.',
    motionBody:
      'The desktop app supports six movement styles and stays within screen bounds while it roams. The site mirrors that motion language without pretending to control your real system cursor.',
    demoEyebrow: 'Interactive preview',
    demoTitle: 'Pick the exact shape and preview the motion.',
    demoBody:
      'Browsers cannot move your real system mouse, so this page focuses on the experience, the motion language, and the brand story.',
    browserDemo: 'Browser demo',
    demoChooserTitle: 'Choose a path and watch the cursor trace it.',
    settingsEyebrow: 'Settings preview',
    settingsTitle: 'Tune the site the way the desktop app should feel.',
    settingsBody:
      'Theme, language, palette, and motion controls are all here so the website feels closer to a real product surface instead of a static landing page.',
    appearance: 'Appearance',
    theme: 'Theme',
    vibe: 'Color vibe',
    language: 'Language',
    comfort: 'Comfort',
    motionSpeed: 'Demo speed',
    motionSpeedLabel: 'Playback',
    reduceMotion: 'Reduce motion',
    highContrast: 'High contrast',
    on: 'On',
    off: 'Off',
    themeOptions: { auto: 'Auto', light: 'Light', dark: 'Dark' },
    vibeOptions: { porcelain: 'Porcelain', tide: 'Tide', ember: 'Ember', dusk: 'Dusk' },
    languageOptions: { en: 'English', es: 'Spanish', pt: 'Portuguese' },
    speedOptions: { slow: 'Slow', normal: 'Normal', brisk: 'Brisk' },
    ctaEyebrow: 'Desktop delivery',
    ctaTitle: 'The next step is a proper GitHub release for the Windows build.',
    ctaBody:
      'The clean way to distribute the app is through GitHub Releases. That gives you a real download page without bloating the repo with a large executable.',
    openReleases: 'Open releases',
    faqEyebrow: 'FAQ',
    faqTitle: 'Common questions',
    faqs: [
      {
        question: 'Can the website move my real mouse?',
        answer:
          'No. Browsers cannot control your system mouse. The website is for the brand, product story, and a live visual demo. The desktop app is the real utility.',
      },
      {
        question: 'What patterns does WIGGLER support?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram, and Random are all supported in the desktop app and mirrored in the website demo.',
      },
      {
        question: 'What is the best way to distribute the app?',
        answer:
          'GitHub Releases is the cleanest place for the Windows build. It keeps the executable out of normal repo history and gives you a proper download page.',
      },
    ],
    patternLabels: {
      Circle: 'A smooth continuous orbit.',
      Square: 'Straight edges with clean turns.',
      Triangle: 'Three-sided travel with visible peaks.',
      'Figure 8': 'A looping crossover that feels more playful.',
      Parallelogram: 'Offset geometry with a cleaner diagonal stance.',
      Random: 'Roaming points that still stay bounded.',
    },
  },
  es: {
    navFeatures: 'Funciones',
    navDemo: 'Demo',
    navSettings: 'Ajustes',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    heroEyebrow: 'Utilidad de escritorio, ahora con un hogar en la web',
    heroTitle: 'Mantiene la sesion activa sin pelear con la persona.',
    heroBody:
      'WIGGLER by SMBA es una app ligera para Windows que espera el tiempo inactivo y luego mueve el cursor en formas limpias y limitadas hasta que vuelves a tocar el mouse.',
    watchDemo: 'Ver demo',
    desktopDownloads: 'Descargas de escritorio',
    bestFor: 'Ideal para',
    bestForValue: 'Flujos de trabajo en Windows',
    corePromise: 'Promesa central',
    corePromiseValue: 'Primero inactivo. Respuesta inmediata.',
    status: 'Estado',
    armed: 'Listo',
    idleDelay: 'Espera inactiva',
    pattern: 'Patron',
    tray: 'Bandeja',
    ready: 'Lista',
    surfaceFootOne: 'La app de escritorio solo toma control despues de la inactividad.',
    surfaceFootTwo: 'Tu mouse recupera el control al instante.',
    featureEyebrow: 'Funcion',
    features: [
      {
        title: 'Control basado en inactividad',
        body: 'WIGGLER permanece preparado en segundo plano y solo empieza a moverse cuando de verdad has estado inactivo.',
      },
      {
        title: 'Devolucion inmediata',
        body: 'En cuanto tocas el mouse, la app de escritorio se detiene y te devuelve el control al instante.',
      },
      {
        title: 'Movimiento acotado',
        body: 'Los patrones siguen siendo visibles y permanecen en pantalla sin salirse del borde.',
      },
    ],
    motionEyebrow: 'Biblioteca de movimiento',
    motionTitle: 'Patrones visibles, comportamiento discreto.',
    motionBody:
      'La app de escritorio ofrece seis estilos de movimiento y se mantiene dentro de los limites de la pantalla. El sitio refleja ese lenguaje visual sin fingir que controla tu cursor real.',
    demoEyebrow: 'Vista interactiva',
    demoTitle: 'Elige la forma exacta y mira el recorrido.',
    demoBody:
      'Los navegadores no pueden mover tu mouse real, por eso esta pagina se centra en la experiencia, el lenguaje de movimiento y la marca.',
    browserDemo: 'Demo web',
    demoChooserTitle: 'Elige una ruta y mira como el cursor la recorre.',
    settingsEyebrow: 'Vista de ajustes',
    settingsTitle: 'Ajusta el sitio como deberia sentirse la app.',
    settingsBody:
      'Tema, idioma, paleta y controles de movimiento estan aqui para que el sitio se sienta mas como un producto real y menos como una pagina estatica.',
    appearance: 'Apariencia',
    theme: 'Tema',
    vibe: 'Paleta',
    language: 'Idioma',
    comfort: 'Comodidad',
    motionSpeed: 'Velocidad del demo',
    motionSpeedLabel: 'Reproduccion',
    reduceMotion: 'Reducir movimiento',
    highContrast: 'Alto contraste',
    on: 'Si',
    off: 'No',
    themeOptions: { auto: 'Auto', light: 'Claro', dark: 'Oscuro' },
    vibeOptions: { porcelain: 'Porcelain', tide: 'Tide', ember: 'Ember', dusk: 'Dusk' },
    languageOptions: { en: 'Ingles', es: 'Espanol', pt: 'Portugues' },
    speedOptions: { slow: 'Lenta', normal: 'Normal', brisk: 'Rapida' },
    ctaEyebrow: 'Entrega de escritorio',
    ctaTitle: 'El siguiente paso es una publicacion real en GitHub para Windows.',
    ctaBody:
      'La forma mas limpia de distribuir la app es con GitHub Releases. Asi obtienes una pagina real de descarga sin inflar el historial del repositorio.',
    openReleases: 'Abrir releases',
    faqEyebrow: 'FAQ',
    faqTitle: 'Preguntas comunes',
    faqs: [
      {
        question: 'La web puede mover mi mouse real?',
        answer:
          'No. Los navegadores no pueden controlar el mouse del sistema. La web sirve para la marca, la historia del producto y una demo visual. La app de escritorio es la utilidad real.',
      },
      {
        question: 'Que patrones soporta WIGGLER?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram y Random estan soportados en la app de escritorio y tambien aparecen en la demo del sitio.',
      },
      {
        question: 'Cual es la mejor forma de distribuir la app?',
        answer:
          'GitHub Releases es el lugar mas limpio para la version de Windows. Mantiene el ejecutable fuera del historial normal del repo y te da una pagina de descarga adecuada.',
      },
    ],
    patternLabels: {
      Circle: 'Una orbita continua y suave.',
      Square: 'Lados rectos con giros definidos.',
      Triangle: 'Recorrido de tres lados con picos visibles.',
      'Figure 8': 'Un cruce en bucle con un tono mas jugueton.',
      Parallelogram: 'Geometria inclinada con una postura mas limpia.',
      Random: 'Puntos sueltos que aun se mantienen dentro del marco.',
    },
  },
  pt: {
    navFeatures: 'Recursos',
    navDemo: 'Demo',
    navSettings: 'Ajustes',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    heroEyebrow: 'Utilitario de desktop, agora com casa na web',
    heroTitle: 'Mantem a sessao ativa sem brigar com a pessoa.',
    heroBody:
      'WIGGLER by SMBA e um app leve para Windows que espera o tempo ocioso e depois move o cursor em formas limpas e contidas ate voce tocar o mouse outra vez.',
    watchDemo: 'Ver demo',
    desktopDownloads: 'Downloads desktop',
    bestFor: 'Melhor para',
    bestForValue: 'Fluxos de trabalho no Windows',
    corePromise: 'Promessa central',
    corePromiseValue: 'Ocioso primeiro. Controle imediato.',
    status: 'Status',
    armed: 'Pronto',
    idleDelay: 'Espera ociosa',
    pattern: 'Padrao',
    tray: 'Bandeja',
    ready: 'Pronta',
    surfaceFootOne: 'O app desktop so assume depois da inatividade.',
    surfaceFootTwo: 'Seu mouse retoma o controle na hora.',
    featureEyebrow: 'Recurso',
    features: [
      {
        title: 'Controle por inatividade',
        body: 'WIGGLER fica armado em segundo plano e so comeca a mover quando voce realmente fica ocioso.',
      },
      {
        title: 'Retorno imediato',
        body: 'Assim que voce toca o mouse, o app desktop para e devolve o controle imediatamente.',
      },
      {
        title: 'Movimento contido',
        body: 'Os padroes permanecem visiveis e dentro da tela em vez de escapar pelas bordas.',
      },
    ],
    motionEyebrow: 'Biblioteca de movimento',
    motionTitle: 'Padroes visiveis, comportamento discreto.',
    motionBody:
      'O app desktop suporta seis estilos de movimento e permanece dentro dos limites da tela. O site espelha essa linguagem visual sem fingir controlar seu cursor real.',
    demoEyebrow: 'Previa interativa',
    demoTitle: 'Escolha a forma exata e veja o movimento.',
    demoBody:
      'Navegadores nao podem mover seu mouse real, entao esta pagina foca na experiencia, na linguagem de movimento e na marca.',
    browserDemo: 'Demo web',
    demoChooserTitle: 'Escolha um trajeto e veja o cursor seguir o caminho.',
    settingsEyebrow: 'Previa de ajustes',
    settingsTitle: 'Ajuste o site como o app deveria parecer.',
    settingsBody:
      'Tema, idioma, paleta e controles de movimento ficam aqui para o site parecer mais um produto real e menos uma pagina estatica.',
    appearance: 'Aparencia',
    theme: 'Tema',
    vibe: 'Paleta',
    language: 'Idioma',
    comfort: 'Conforto',
    motionSpeed: 'Velocidade da demo',
    motionSpeedLabel: 'Ritmo',
    reduceMotion: 'Reduzir movimento',
    highContrast: 'Alto contraste',
    on: 'Ligado',
    off: 'Desligado',
    themeOptions: { auto: 'Auto', light: 'Claro', dark: 'Escuro' },
    vibeOptions: { porcelain: 'Porcelain', tide: 'Tide', ember: 'Ember', dusk: 'Dusk' },
    languageOptions: { en: 'Ingles', es: 'Espanhol', pt: 'Portugues' },
    speedOptions: { slow: 'Lento', normal: 'Normal', brisk: 'Rapido' },
    ctaEyebrow: 'Entrega desktop',
    ctaTitle: 'O proximo passo e uma release de verdade no GitHub para Windows.',
    ctaBody:
      'A forma mais limpa de distribuir o app e com GitHub Releases. Isso cria uma pagina real de download sem inflar o historico do repositorio.',
    openReleases: 'Abrir releases',
    faqEyebrow: 'FAQ',
    faqTitle: 'Perguntas comuns',
    faqs: [
      {
        question: 'O site pode mover meu mouse real?',
        answer:
          'Nao. Navegadores nao podem controlar o mouse do sistema. O site existe para a marca, a historia do produto e uma demo visual. O app desktop e a utilidade real.',
      },
      {
        question: 'Quais padroes o WIGGLER suporta?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram e Random existem no app desktop e tambem aparecem na demo do site.',
      },
      {
        question: 'Qual e a melhor forma de distribuir o app?',
        answer:
          'GitHub Releases e o lugar mais limpo para a versao de Windows. Mantem o executavel fora do historico normal do repo e oferece uma pagina de download adequada.',
      },
    ],
    patternLabels: {
      Circle: 'Uma orbita continua e suave.',
      Square: 'Lados retos com curvas bem marcadas.',
      Triangle: 'Tres lados com picos bem visiveis.',
      'Figure 8': 'Um cruzamento em loop com clima mais brincalhao.',
      Parallelogram: 'Geometria inclinada com uma postura mais limpa.',
      Random: 'Pontos livres que ainda ficam dentro da moldura.',
    },
  },
}

const storageKeys = {
  theme: 'wiggler-theme',
  vibe: 'wiggler-vibe',
  language: 'wiggler-language',
  speed: 'wiggler-speed',
  reduceMotion: 'wiggler-reduce-motion',
  highContrast: 'wiggler-high-contrast',
}

function getInitialLanguage() {
  const stored = window.localStorage.getItem(storageKeys.language)
  if (stored && copy[stored]) {
    return stored
  }

  const browserLanguage = window.navigator.language.toLowerCase()
  if (browserLanguage.startsWith('es')) {
    return 'es'
  }
  if (browserLanguage.startsWith('pt')) {
    return 'pt'
  }

  return 'en'
}

function resolveTheme(theme) {
  if (theme !== 'auto') {
    return theme
  }

  return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
}

function PatternDemo({ language, speed, reduceMotion }) {
  const [selectedPattern, setSelectedPattern] = useState('Circle')
  const activePattern = patternConfig[selectedPattern]
  const text = copy[language]
  const speedMultiplier = speed === 'slow' ? 1.35 : speed === 'brisk' ? 0.82 : 1
  const animationDuration = `${(reduceMotion ? activePattern.duration * 2.3 : activePattern.duration * speedMultiplier).toFixed(1)}s`

  return (
    <div className="demo-card">
      <div className="demo-grid" aria-hidden="true">
        <span />
        <span />
        <span />
        <span />
      </div>

      <div className="demo-toolbar">
        <div>
          <p className="eyebrow">{text.browserDemo}</p>
          <h3>{text.demoChooserTitle}</h3>
          <p>{text.patternLabels[selectedPattern]}</p>
        </div>

        <div className="demo-pills" role="tablist" aria-label="Choose a cursor pattern">
          {patterns.map((pattern) => (
            <button
              key={pattern}
              type="button"
              className={`demo-pill ${pattern === selectedPattern ? 'is-active' : ''}`}
              onClick={() => setSelectedPattern(pattern)}
            >
              {pattern}
            </button>
          ))}
        </div>
      </div>

      <svg
        key={`${selectedPattern}-${animationDuration}`}
        className="demo-svg"
        viewBox="0 0 640 420"
        role="img"
        aria-label={`Animated preview of the ${selectedPattern} cursor pattern`}
      >
        <defs>
          <path id="selected-path" d={activePattern.motionPath} />
        </defs>

        <path className="demo-trail" d="M 92 164 C 132 204, 178 116, 236 170 S 332 230, 392 178 S 488 116, 548 166" />
        {activePattern.outline}

        <g className="cursor-group">
          {!reduceMotion && (
            <animateMotion dur={animationDuration} repeatCount="indefinite" rotate="auto">
              <mpath href="#selected-path" />
            </animateMotion>
          )}
          <path
            className="cursor-shape"
            d="M0 0 L40 16 L18 26 L28 46 L16 52 L6 30 L-2 48 L-10 44 L-2 18 Z"
          />
          <path className="cursor-ring" d="M 8 -20 Q 18 -34 34 -34" />
          <path className="cursor-ring delay" d="M 18 -8 Q 32 -24 48 -24" />
        </g>
      </svg>
    </div>
  )
}

function SettingsSection({
  language,
  setLanguage,
  theme,
  setTheme,
  vibe,
  setVibe,
  speed,
  setSpeed,
  reduceMotion,
  setReduceMotion,
  highContrast,
  setHighContrast,
}) {
  const text = copy[language]

  return (
    <section className="settings-section" id="settings">
      <div className="section-heading">
        <p className="eyebrow">{text.settingsEyebrow}</p>
        <h2>{text.settingsTitle}</h2>
        <p>{text.settingsBody}</p>
      </div>

      <div className="settings-grid">
        <article className="settings-card">
          <p className="eyebrow">{text.appearance}</p>
          <div className="setting-group">
            <label htmlFor="theme-select">{text.theme}</label>
            <select id="theme-select" value={theme} onChange={(event) => setTheme(event.target.value)}>
              <option value="auto">{text.themeOptions.auto}</option>
              <option value="light">{text.themeOptions.light}</option>
              <option value="dark">{text.themeOptions.dark}</option>
            </select>
          </div>
          <div className="setting-group">
            <label htmlFor="vibe-select">{text.vibe}</label>
            <select id="vibe-select" value={vibe} onChange={(event) => setVibe(event.target.value)}>
              <option value="porcelain">{text.vibeOptions.porcelain}</option>
              <option value="tide">{text.vibeOptions.tide}</option>
              <option value="ember">{text.vibeOptions.ember}</option>
              <option value="dusk">{text.vibeOptions.dusk}</option>
            </select>
          </div>
          <div className="setting-group">
            <label htmlFor="language-select">{text.language}</label>
            <select id="language-select" value={language} onChange={(event) => setLanguage(event.target.value)}>
              <option value="en">{text.languageOptions.en}</option>
              <option value="es">{text.languageOptions.es}</option>
              <option value="pt">{text.languageOptions.pt}</option>
            </select>
          </div>
        </article>

        <article className="settings-card">
          <p className="eyebrow">{text.comfort}</p>
          <div className="setting-group">
            <label htmlFor="speed-select">{text.motionSpeed}</label>
            <select id="speed-select" value={speed} onChange={(event) => setSpeed(event.target.value)}>
              <option value="slow">{text.speedOptions.slow}</option>
              <option value="normal">{text.speedOptions.normal}</option>
              <option value="brisk">{text.speedOptions.brisk}</option>
            </select>
          </div>
          <div className="toggle-row">
            <div>
              <strong>{text.reduceMotion}</strong>
              <p>{reduceMotion ? text.on : text.off}</p>
            </div>
            <button
              type="button"
              className={`toggle-button ${reduceMotion ? 'is-active' : ''}`}
              onClick={() => setReduceMotion((current) => !current)}
              aria-pressed={reduceMotion}
            >
              <span />
            </button>
          </div>
          <div className="toggle-row">
            <div>
              <strong>{text.highContrast}</strong>
              <p>{highContrast ? text.on : text.off}</p>
            </div>
            <button
              type="button"
              className={`toggle-button ${highContrast ? 'is-active' : ''}`}
              onClick={() => setHighContrast((current) => !current)}
              aria-pressed={highContrast}
            >
              <span />
            </button>
          </div>
        </article>
      </div>
    </section>
  )
}

function App() {
  const [theme, setTheme] = useState(() => window.localStorage.getItem(storageKeys.theme) ?? 'auto')
  const [vibe, setVibe] = useState(() => window.localStorage.getItem(storageKeys.vibe) ?? 'porcelain')
  const [language, setLanguage] = useState(getInitialLanguage)
  const [speed, setSpeed] = useState(() => window.localStorage.getItem(storageKeys.speed) ?? 'normal')
  const [reduceMotion, setReduceMotion] = useState(
    () => window.localStorage.getItem(storageKeys.reduceMotion) === 'true',
  )
  const [highContrast, setHighContrast] = useState(
    () => window.localStorage.getItem(storageKeys.highContrast) === 'true',
  )

  useEffect(() => {
    const root = document.documentElement
    const applyTheme = () => {
      root.dataset.theme = resolveTheme(theme)
      root.dataset.vibe = vibe
      root.dataset.contrast = highContrast ? 'high' : 'normal'
      root.dataset.motion = reduceMotion ? 'reduced' : 'full'
    }

    applyTheme()
    window.localStorage.setItem(storageKeys.theme, theme)
    window.localStorage.setItem(storageKeys.vibe, vibe)
    window.localStorage.setItem(storageKeys.language, language)
    window.localStorage.setItem(storageKeys.speed, speed)
    window.localStorage.setItem(storageKeys.reduceMotion, String(reduceMotion))
    window.localStorage.setItem(storageKeys.highContrast, String(highContrast))

    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)')
    const handleChange = () => {
      if (theme === 'auto') {
        applyTheme()
      }
    }

    mediaQuery.addEventListener('change', handleChange)

    return () => {
      mediaQuery.removeEventListener('change', handleChange)
    }
  }, [theme, vibe, language, speed, reduceMotion, highContrast])

  const text = copy[language]

  return (
    <div className="site-shell">
      <header className="topbar">
        <div className="brand-lockup" aria-label="WIGGLER by SMBA">
          <div className="brand-stack">
            <div className="brand-mark" aria-hidden="true">
              <svg viewBox="0 0 220 108">
                <path d="M 18 52 C 38 74, 60 28, 88 52 S 140 74, 170 52" />
                <path d="M 162 18 Q 174 2 194 4" />
                <path d="M 168 30 Q 184 12 204 18" />
                <path d="M 170 20 L 214 40 L 188 56 L 208 78 L 196 90 L 176 66 L 162 92 L 146 84 L 160 48 Z" />
              </svg>
            </div>
            <div className="wordmark-wrap">
              <p className="wordmark">WIGGLER</p>
              <p className="submark">by smba</p>
            </div>
          </div>
        </div>

        <nav className="topnav" aria-label="Primary">
          <a href="#features">{text.navFeatures}</a>
          <a href="#demo">{text.navDemo}</a>
          <a href="#settings">{text.navSettings}</a>
          <a href="#faq">{text.navFaq}</a>
          <a className="button ghost" href="https://github.com/smba11/Wiggler" target="_blank" rel="noreferrer">
            {text.navGithub}
          </a>
        </nav>
      </header>

      <main>
        <section className="hero">
          <div className="hero-copy">
            <p className="eyebrow">{text.heroEyebrow}</p>
            <h1>{text.heroTitle}</h1>
            <p className="hero-body">{text.heroBody}</p>

            <div className="hero-actions">
              <a className="button primary" href="#demo">
                {text.watchDemo}
              </a>
              <a
                className="button ghost"
                href="https://github.com/smba11/Wiggler/releases"
                target="_blank"
                rel="noreferrer"
              >
                {text.desktopDownloads}
              </a>
            </div>

            <div className="hero-meta">
              <div>
                <span className="meta-label">{text.bestFor}</span>
                <strong>{text.bestForValue}</strong>
              </div>
              <div>
                <span className="meta-label">{text.corePromise}</span>
                <strong>{text.corePromiseValue}</strong>
              </div>
            </div>
          </div>

          <div className="hero-panel">
            <div className="hero-surface">
              <div className="surface-status">
                <span>{text.status}</span>
                <strong>{text.armed}</strong>
              </div>
              <div className="surface-stats">
                <article>
                  <span>{text.idleDelay}</span>
                  <strong>12 sec</strong>
                </article>
                <article>
                  <span>{text.pattern}</span>
                  <strong>Circle</strong>
                </article>
                <article>
                  <span>{text.tray}</span>
                  <strong>{text.ready}</strong>
                </article>
              </div>
              <div className="surface-foot">
                <span>{text.surfaceFootOne}</span>
                <span>{text.surfaceFootTwo}</span>
              </div>
            </div>
          </div>
        </section>

        <section className="feature-strip" id="features">
          {text.features.map((feature) => (
            <article className="feature-card" key={feature.title}>
              <p className="eyebrow">{text.featureEyebrow}</p>
              <h2>{feature.title}</h2>
              <p>{feature.body}</p>
            </article>
          ))}
        </section>

        <section className="patterns-section">
          <div className="patterns-copy">
            <p className="eyebrow">{text.motionEyebrow}</p>
            <h2>{text.motionTitle}</h2>
            <p>{text.motionBody}</p>
          </div>

          <div className="pattern-pills" aria-label="Supported patterns">
            {patterns.map((pattern) => (
              <span key={pattern}>{pattern}</span>
            ))}
          </div>
        </section>

        <section className="demo-section" id="demo">
          <div className="section-heading">
            <p className="eyebrow">{text.demoEyebrow}</p>
            <h2>{text.demoTitle}</h2>
            <p>{text.demoBody}</p>
          </div>
          <PatternDemo language={language} speed={speed} reduceMotion={reduceMotion} />
        </section>

        <SettingsSection
          language={language}
          setLanguage={setLanguage}
          theme={theme}
          setTheme={setTheme}
          vibe={vibe}
          setVibe={setVibe}
          speed={speed}
          setSpeed={setSpeed}
          reduceMotion={reduceMotion}
          setReduceMotion={setReduceMotion}
          highContrast={highContrast}
          setHighContrast={setHighContrast}
        />

        <section className="cta-section">
          <div className="cta-card">
            <div>
              <p className="eyebrow">{text.ctaEyebrow}</p>
              <h2>{text.ctaTitle}</h2>
              <p>{text.ctaBody}</p>
            </div>
            <a
              className="button primary"
              href="https://github.com/smba11/Wiggler/releases"
              target="_blank"
              rel="noreferrer"
            >
              {text.openReleases}
            </a>
          </div>
        </section>

        <section className="faq-section" id="faq">
          <div className="section-heading">
            <p className="eyebrow">{text.faqEyebrow}</p>
            <h2>{text.faqTitle}</h2>
          </div>
          <div className="faq-list">
            {text.faqs.map((item) => (
              <article className="faq-item" key={item.question}>
                <h3>{item.question}</h3>
                <p>{item.answer}</p>
              </article>
            ))}
          </div>
        </section>
      </main>
    </div>
  )
}

export default App
