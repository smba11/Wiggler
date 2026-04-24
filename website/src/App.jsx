import './App.css'

const patterns = ['Circle', 'Square', 'Triangle', 'Figure 8', 'Parallelogram', 'Random']

const features = [
  {
    title: 'Idle-first control',
    body: 'WIGGLER stays armed in the background and only starts moving after you have been idle.',
  },
  {
    title: 'Instant handoff',
    body: 'The second you touch the mouse, the desktop app stops and gives full control back.',
  },
  {
    title: 'Bounded motion',
    body: 'Patterns stay visible, on-screen, and deliberate instead of drifting off the edge.',
  },
]

const faqs = [
  {
    question: 'Can the website move my real mouse?',
    answer:
      'No. Browsers cannot control your system mouse. The website is for the brand, product story, and a visual demo. The desktop app is the real utility.',
  },
  {
    question: 'What patterns does WIGGLER support?',
    answer:
      'Circle, Square, Triangle, Figure 8, Parallelogram, and Random are all supported in the desktop app.',
  },
  {
    question: 'Does it keep running in the tray?',
    answer:
      'Yes. The desktop app can launch in the tray, minimize to the tray, and keep running while the main window is hidden.',
  },
]

function PatternDemo() {
  return (
    <div className="demo-card">
      <div className="demo-grid" aria-hidden="true">
        <span />
        <span />
        <span />
        <span />
      </div>

      <svg
        className="demo-svg"
        viewBox="0 0 640 420"
        role="img"
        aria-label="Animated preview of cursor motion patterns inside the browser"
      >
        <defs>
          <path
            id="circle-path"
            d="M 184 210
               a 82 82 0 1 1 164 0
               a 82 82 0 1 1 -164 0"
          />
          <path
            id="square-path"
            d="M 378 128 L 508 128 L 508 292 L 378 292 Z"
          />
          <path
            id="trail-path"
            d="M 96 178
               C 142 224, 184 118, 238 178
               S 334 236, 390 186
               S 482 114, 546 178"
          />
        </defs>

        <path className="demo-trail" d="M 96 178 C 142 224, 184 118, 238 178 S 334 236, 390 186 S 482 114, 546 178" />

        <g className="orbit orbit-circle">
          <circle className="path-stroke" cx="266" cy="210" r="82" />
          <g className="cursor-group">
            <animateMotion dur="8s" repeatCount="indefinite" rotate="auto">
              <mpath href="#circle-path" />
            </animateMotion>
            <path
              className="cursor-shape"
              d="M0 0 L40 16 L18 26 L28 46 L16 52 L6 30 L-2 48 L-10 44 L-2 18 Z"
            />
            <path className="cursor-ring" d="M 8 -20 Q 18 -34 34 -34" />
            <path className="cursor-ring delay" d="M 18 -8 Q 32 -24 48 -24" />
          </g>
        </g>

        <g className="orbit orbit-square">
          <path className="path-stroke" d="M 378 128 L 508 128 L 508 292 L 378 292 Z" />
          <g className="cursor-group secondary">
            <animateMotion dur="10s" repeatCount="indefinite" rotate="auto">
              <mpath href="#square-path" />
            </animateMotion>
            <path
              className="cursor-shape"
              d="M0 0 L30 12 L14 20 L22 36 L13 40 L6 24 L0 38 L-7 34 L0 14 Z"
            />
          </g>
        </g>

        <g className="orbit orbit-random">
          <circle className="demo-dot demo-dot-a" cx="144" cy="308" r="7" />
          <circle className="demo-dot demo-dot-b" cx="256" cy="332" r="7" />
          <circle className="demo-dot demo-dot-c" cx="480" cy="98" r="7" />
        </g>
      </svg>

      <div className="demo-copy">
        <p className="eyebrow">Browser demo</p>
        <h3>The site shows the feel. The desktop app does the real work.</h3>
        <p>
          This preview simulates smooth cursor paths in the browser. The actual Windows utility is
          what arms itself, waits for idle time, and takes over only when you have stepped away.
        </p>
      </div>
    </div>
  )
}

function App() {
  return (
    <div className="site-shell">
      <header className="topbar">
        <div className="brand-lockup" aria-label="WIGGLER by SMBA">
          <div className="brand-mark" aria-hidden="true">
            <svg viewBox="0 0 180 80">
              <path d="M 8 42 C 28 62, 46 20, 70 42 S 112 62, 136 42" />
              <path d="M 134 18 Q 144 6 158 8" />
              <path d="M 142 24 Q 154 12 168 16" />
              <path d="M 146 18 L 176 30 L 158 40 L 168 56 L 156 62 L 144 42 L 136 58 L 126 54 L 132 34 Z" />
            </svg>
          </div>
          <div>
            <p className="wordmark">WIGGLER</p>
            <p className="submark">by smba</p>
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
                href="https://github.com/smba11/Wiggler"
                target="_blank"
                rel="noreferrer"
              >
                View GitHub repo
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
            <h2>A web demo for the idea. A desktop app for the job.</h2>
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
              <p className="eyebrow">Open source home</p>
              <h2>Build the brand here. Ship the utility from the desktop app.</h2>
              <p>
                The repo can now hold both pieces: the Windows utility and this website. That makes
                GitHub the clean place for docs, screenshots, release notes, and future downloads.
              </p>
            </div>
            <a
              className="button primary"
              href="https://github.com/smba11/Wiggler"
              target="_blank"
              rel="noreferrer"
            >
              Open the repo
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
