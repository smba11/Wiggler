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

const baseCopy = {
  navFeatures: 'Features',
  navDemo: 'Demo',
  navFaq: 'FAQ',
  navGithub: 'GitHub',
  settings: 'Settings',
  theme: 'Theme',
  language: 'Language',
  light: 'Light',
  dark: 'Dark',
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
}

const languageOptions = {
  en: 'English',
  es: 'Espanol',
  pt: 'Portugues',
  fr: 'Francais',
  de: 'Deutsch',
  it: 'Italiano',
  nl: 'Nederlands',
  sv: 'Svenska',
  ja: 'Japanese',
  ko: 'Korean',
  zh: 'Chinese',
  ar: 'Arabic',
  hi: 'Hindi',
}

const languageOverrides = {
  es: {
    navFeatures: 'Funciones',
    navFaq: 'Preguntas',
    settings: 'Ajustes',
    theme: 'Tema',
    language: 'Idioma',
    light: 'Claro',
    dark: 'Oscuro',
    watchDemo: 'Ver demo',
    desktopDownloads: 'Descargas',
  },
  pt: {
    navFeatures: 'Recursos',
    navFaq: 'Perguntas',
    settings: 'Ajustes',
    theme: 'Tema',
    language: 'Idioma',
    light: 'Claro',
    dark: 'Escuro',
    watchDemo: 'Ver demo',
    desktopDownloads: 'Downloads',
  },
  fr: {
    navFeatures: 'Fonctions',
    settings: 'Parametres',
    theme: 'Theme',
    language: 'Langue',
    light: 'Clair',
    dark: 'Sombre',
    watchDemo: 'Voir la demo',
    desktopDownloads: 'Telechargements',
  },
  de: {
    navFeatures: 'Funktionen',
    settings: 'Einstellungen',
    theme: 'Design',
    language: 'Sprache',
    light: 'Hell',
    dark: 'Dunkel',
    watchDemo: 'Demo ansehen',
    desktopDownloads: 'Downloads',
  },
  it: {
    navFeatures: 'Funzioni',
    settings: 'Impostazioni',
    theme: 'Tema',
    language: 'Lingua',
    light: 'Chiaro',
    dark: 'Scuro',
    watchDemo: 'Guarda demo',
    desktopDownloads: 'Download',
  },
  nl: {
    navFeatures: 'Functies',
    settings: 'Instellingen',
    theme: 'Thema',
    language: 'Taal',
    light: 'Licht',
    dark: 'Donker',
  },
  sv: {
    navFeatures: 'Funktioner',
    settings: 'Installningar',
    theme: 'Tema',
    language: 'Sprak',
    light: 'Ljust',
    dark: 'Morkt',
  },
  ja: {
    navFeatures: 'Features',
    settings: 'Settings',
    theme: 'Theme',
    language: 'Language',
    light: 'Light',
    dark: 'Dark',
  },
  ko: {
    navFeatures: 'Features',
    settings: 'Settings',
    theme: 'Theme',
    language: 'Language',
    light: 'Light',
    dark: 'Dark',
  },
  zh: {
    navFeatures: 'Features',
    settings: 'Settings',
    theme: 'Theme',
    language: 'Language',
    light: 'Light',
    dark: 'Dark',
  },
  ar: {
    navFeatures: 'Features',
    settings: 'Settings',
    theme: 'Theme',
    language: 'Language',
    light: 'Light',
    dark: 'Dark',
  },
  hi: {
    navFeatures: 'Features',
    settings: 'Settings',
    theme: 'Theme',
    language: 'Language',
    light: 'Light',
    dark: 'Dark',
  },
}

const storageKeys = {
  theme: 'wiggler-theme',
  language: 'wiggler-language',
}

function getInitialLanguage() {
  const stored = window.localStorage.getItem(storageKeys.language)
  if (stored && languageOptions[stored]) {
    return stored
  }

  const browserLanguage = window.navigator.language.toLowerCase()
  const match = Object.keys(languageOptions).find((code) => browserLanguage.startsWith(code))
  return match ?? 'en'
}

function getCopy(language) {
  return {
    ...baseCopy,
    ...languageOverrides[language],
    languageOptions,
  }
}

function PatternDemo({ language }) {
  const [selectedPattern, setSelectedPattern] = useState('Circle')
  const activePattern = patternConfig[selectedPattern]
  const text = getCopy(language)

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
        key={selectedPattern}
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
          <animateMotion dur={`${activePattern.duration}s`} repeatCount="indefinite" rotate="auto">
            <mpath href="#selected-path" />
          </animateMotion>
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

function SettingsPopover({ language, setLanguage, theme, setTheme, isOpen, setIsOpen }) {
  const text = getCopy(language)

  return (
    <div className="settings-wrap">
      <button
        type="button"
        className={`settings-trigger ${isOpen ? 'is-active' : ''}`}
        aria-label={text.settings}
        aria-expanded={isOpen}
        onClick={() => setIsOpen((current) => !current)}
      >
        <svg viewBox="0 0 24 24" aria-hidden="true">
          <path d="M12 3.75 13.62 5.1l2.08-.31.93 1.89 1.99.69-.1 2.11 1.48 1.49-1.11 1.8 1.11 1.8-1.48 1.49.1 2.11-1.99.69-.93 1.89-2.08-.31L12 20.25l-1.62-1.35-2.08.31-.93-1.89-1.99-.69.1-2.11-1.48-1.49 1.11-1.8-1.11-1.8 1.48-1.49-.1-2.11 1.99-.69.93-1.89 2.08.31L12 3.75Z" />
          <circle cx="12" cy="12" r="3.3" />
        </svg>
      </button>

      {isOpen && (
        <div className="settings-popover">
          <div className="setting-group compact">
            <label htmlFor="theme-select">{text.theme}</label>
            <select id="theme-select" value={theme} onChange={(event) => setTheme(event.target.value)}>
              <option value="light">{text.light}</option>
              <option value="dark">{text.dark}</option>
            </select>
          </div>

          <div className="setting-group compact">
            <label htmlFor="language-select">{text.language}</label>
            <select id="language-select" value={language} onChange={(event) => setLanguage(event.target.value)}>
              {Object.entries(text.languageOptions).map(([code, label]) => (
                <option key={code} value={code}>
                  {label}
                </option>
              ))}
            </select>
          </div>
        </div>
      )}
    </div>
  )
}

function App() {
  const [theme, setTheme] = useState(() => window.localStorage.getItem(storageKeys.theme) ?? 'light')
  const [language, setLanguage] = useState(getInitialLanguage)
  const [isSettingsOpen, setIsSettingsOpen] = useState(false)

  useEffect(() => {
    document.documentElement.dataset.theme = theme
    window.localStorage.setItem(storageKeys.theme, theme)
    window.localStorage.setItem(storageKeys.language, language)
  }, [theme, language])

  const text = getCopy(language)

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

        <div className="topbar-actions">
          <nav className="topnav" aria-label="Primary">
            <a href="#features">{text.navFeatures}</a>
            <a href="#demo">{text.navDemo}</a>
            <a href="#faq">{text.navFaq}</a>
            <a className="button ghost" href="https://github.com/smba11/Wiggler" target="_blank" rel="noreferrer">
              {text.navGithub}
            </a>
          </nav>

          <SettingsPopover
            language={language}
            setLanguage={setLanguage}
            theme={theme}
            setTheme={setTheme}
            isOpen={isSettingsOpen}
            setIsOpen={setIsSettingsOpen}
          />
        </div>
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
          <PatternDemo language={language} />
        </section>

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
