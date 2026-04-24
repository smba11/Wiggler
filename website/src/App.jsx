import { useState } from 'react'
import './App.css'

const patterns = ['Circle', 'Square', 'Triangle', 'Figure 8', 'Parallelogram', 'Random']

const patternConfig = {
  Circle: {
    outline: <circle className="path-stroke" cx="320" cy="196" r="94" />,
    motionPath: 'M 226 196 a 94 94 0 1 1 188 0 a 94 94 0 1 1 -188 0',
    duration: '7.5s',
    label: 'A smooth continuous orbit.',
  },
  Square: {
    outline: <path className="path-stroke" d="M 218 104 L 422 104 L 422 308 L 218 308 Z" />,
    motionPath: 'M 218 104 L 422 104 L 422 308 L 218 308 Z',
    duration: '8.5s',
    label: 'Straight edges with clean turns.',
  },
  Triangle: {
    outline: <path className="path-stroke" d="M 320 92 L 454 310 L 186 310 Z" />,
    motionPath: 'M 320 92 L 454 310 L 186 310 Z',
    duration: '7.8s',
    label: 'Three-sided travel with visible peaks.',
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
    duration: '9.2s',
    label: 'A looping crossover that feels more playful.',
  },
  Parallelogram: {
    outline: <path className="path-stroke" d="M 234 108 L 448 108 L 394 302 L 180 302 Z" />,
    motionPath: 'M 234 108 L 448 108 L 394 302 L 180 302 Z',
    duration: '8.1s',
    label: 'Offset geometry with a cleaner diagonal stance.',
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
    duration: '8.8s',
    label: 'Roaming points that still stay bounded.',
  },
}

const features = [
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
]

const faqs = [
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
]

function PatternDemo() {
  const [selectedPattern, setSelectedPattern] = useState('Circle')
  const activePattern = patternConfig[selectedPattern]

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
          <p className="eyebrow">Browser demo</p>
          <h3>Choose a path and watch the cursor trace it.</h3>
          <p>{activePattern.label}</p>
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
          <path
            id="trail-path"
            d="M 92 164 C 132 204, 178 116, 236 170 S 332 230, 392 178 S 488 116, 548 166"
          />
        </defs>

        <path className="demo-trail" d="M 92 164 C 132 204, 178 116, 236 170 S 332 230, 392 178 S 488 116, 548 166" />
        {activePattern.outline}

        <g className="cursor-group">
          <animateMotion dur={activePattern.duration} repeatCount="indefinite" rotate="auto">
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

function App() {
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
              <div className="submark-row">
                <span />
                <p className="submark">by smba</p>
                <span />
              </div>
            </div>
          </div>
        </div>

        <nav className="topnav" aria-label="Primary">
          <a href="#features">Features</a>
          <a href="#demo">Demo</a>
          <a href="#faq">FAQ</a>
          <a className="button ghost" href="https://github.com/smba11/Wiggler" target="_blank" rel="noreferrer">
            GitHub
          </a>
        </nav>
      </header>

      <main>
        <section className="hero">
          <div className="hero-copy">
            <p className="eyebrow">Desktop utility, now with a home on the web</p>
            <h1>Keep the session alive without fighting the user.</h1>
            <p className="hero-body">
              WIGGLER by SMBA is a lightweight Windows app that waits for idle time, then moves the
              cursor in clean, bounded shapes until you touch the mouse again.
            </p>

            <div className="hero-actions">
              <a className="button primary" href="#demo">
                Watch the demo
              </a>
              <a
                className="button ghost"
                href="https://github.com/smba11/Wiggler/releases"
                target="_blank"
                rel="noreferrer"
              >
                Desktop downloads
              </a>
            </div>

            <div className="hero-meta">
              <div>
                <span className="meta-label">Best for</span>
                <strong>Windows desktop workflows</strong>
              </div>
              <div>
                <span className="meta-label">Core promise</span>
                <strong>Idle first. Instant handoff.</strong>
              </div>
            </div>
          </div>

          <div className="hero-panel">
            <div className="hero-surface">
              <div className="surface-status">
                <span>Status</span>
                <strong>Armed</strong>
              </div>
              <div className="surface-stats">
                <article>
                  <span>Idle delay</span>
                  <strong>12 sec</strong>
                </article>
                <article>
                  <span>Pattern</span>
                  <strong>Circle</strong>
                </article>
                <article>
                  <span>Tray</span>
                  <strong>Ready</strong>
                </article>
              </div>
              <div className="surface-foot">
                <span>The desktop app takes over only after inactivity.</span>
                <span>Your mouse wins instantly.</span>
              </div>
            </div>
          </div>
        </section>

        <section className="feature-strip" id="features">
          {features.map((feature) => (
            <article className="feature-card" key={feature.title}>
              <p className="eyebrow">Feature</p>
              <h2>{feature.title}</h2>
              <p>{feature.body}</p>
            </article>
          ))}
        </section>

        <section className="patterns-section">
          <div className="patterns-copy">
            <p className="eyebrow">Motion library</p>
            <h2>Visible patterns, quiet behavior.</h2>
            <p>
              The desktop app supports six movement styles and stays within screen bounds while it
              roams. The site mirrors that motion language without pretending to control your real
              system cursor.
            </p>
          </div>

          <div className="pattern-pills" aria-label="Supported patterns">
            {patterns.map((pattern) => (
              <span key={pattern}>{pattern}</span>
            ))}
          </div>
        </section>

        <section className="demo-section" id="demo">
          <div className="section-heading">
            <p className="eyebrow">Interactive preview</p>
            <h2>Pick the exact shape and preview the motion.</h2>
            <p>
              Browsers cannot move your real system mouse, so this page focuses on the experience,
              the motion language, and the brand story.
            </p>
          </div>
          <PatternDemo />
        </section>

        <section className="cta-section">
          <div className="cta-card">
            <div>
              <p className="eyebrow">Desktop delivery</p>
              <h2>The next step is a proper GitHub release for the Windows build.</h2>
              <p>
                The clean way to distribute the app is through GitHub Releases. That gives you a real
                download page without bloating the repo with a 170 MB executable.
              </p>
            </div>
            <a
              className="button primary"
              href="https://github.com/smba11/Wiggler/releases"
              target="_blank"
              rel="noreferrer"
            >
              Open releases
            </a>
          </div>
        </section>

        <section className="faq-section" id="faq">
          <div className="section-heading">
            <p className="eyebrow">FAQ</p>
            <h2>Common questions</h2>
          </div>
          <div className="faq-list">
            {faqs.map((item) => (
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
