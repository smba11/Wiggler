import { useEffect, useState } from 'react'
import './App.css'

const settingsIconUrl = `${import.meta.env.BASE_URL}assets/settings-icon.png`

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

const translations = {
  en: {
    languageName: 'English',
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
    armed: 'Ready',
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
    patternNames: {
      Circle: 'Circle',
      Square: 'Square',
      Triangle: 'Triangle',
      'Figure 8': 'Figure 8',
      Parallelogram: 'Parallelogram',
      Random: 'Random',
    },
    demoEyebrow: 'Interactive preview',
    demoTitle: 'Pick the exact shape and preview the motion.',
    demoBody:
      'Browsers cannot move your real system mouse, so this page focuses on the experience, the motion language, and the brand story.',
    browserDemo: 'Browser demo',
    demoChooserTitle: 'Choose a path and watch the cursor trace it.',
    patternLabels: {
      Circle: 'A smooth continuous orbit.',
      Square: 'Straight edges with clean turns.',
      Triangle: 'Three-sided travel with visible peaks.',
      'Figure 8': 'A looping crossover that feels more playful.',
      Parallelogram: 'Offset geometry with a cleaner diagonal stance.',
      Random: 'Roaming points that still stay bounded.',
    },
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
  },
  es: {
    languageName: 'Español',
    navFeatures: 'Funciones',
    navDemo: 'Demo',
    navFaq: 'Preguntas',
    navGithub: 'GitHub',
    settings: 'Ajustes',
    theme: 'Tema',
    language: 'Idioma',
    light: 'Claro',
    dark: 'Oscuro',
    heroEyebrow: 'Utilidad de escritorio, ahora con una casa en la web',
    heroTitle: 'Mantén la sesión activa sin pelear con la persona.',
    heroBody:
      'WIGGLER by SMBA es una app ligera para Windows que espera el tiempo inactivo y luego mueve el cursor en formas limpias y controladas hasta que vuelves a tocar el mouse.',
    watchDemo: 'Ver demo',
    desktopDownloads: 'Descargas desktop',
    bestFor: 'Ideal para',
    bestForValue: 'Flujos de trabajo en Windows',
    corePromise: 'Promesa central',
    corePromiseValue: 'Primero inactivo. Respuesta inmediata.',
    status: 'Estado',
    armed: 'Listo',
    idleDelay: 'Espera inactiva',
    pattern: 'Patrón',
    tray: 'Bandeja',
    ready: 'Lista',
    surfaceFootOne: 'La app de escritorio solo toma control después de la inactividad.',
    surfaceFootTwo: 'Tu mouse recupera el control al instante.',
    featureEyebrow: 'Función',
    features: [
      {
        title: 'Control basado en inactividad',
        body: 'WIGGLER permanece preparado en segundo plano y solo empieza a moverse cuando de verdad has estado inactivo.',
      },
      {
        title: 'Devolución inmediata',
        body: 'En cuanto tocas el mouse, la app de escritorio se detiene y te devuelve el control al instante.',
      },
      {
        title: 'Movimiento acotado',
        body: 'Los patrones siguen siendo visibles, dentro de pantalla y con intención en lugar de perderse en el borde.',
      },
    ],
    motionEyebrow: 'Biblioteca de movimiento',
    motionTitle: 'Patrones visibles, comportamiento discreto.',
    motionBody:
      'La app de escritorio ofrece seis estilos de movimiento y se mantiene dentro de los límites de la pantalla. El sitio refleja ese lenguaje visual sin fingir que controla tu cursor real.',
    patternNames: {
      Circle: 'Círculo',
      Square: 'Cuadrado',
      Triangle: 'Triángulo',
      'Figure 8': 'Figura 8',
      Parallelogram: 'Paralelogramo',
      Random: 'Aleatorio',
    },
    demoEyebrow: 'Vista interactiva',
    demoTitle: 'Elige la forma exacta y mira el recorrido.',
    demoBody:
      'Los navegadores no pueden mover tu mouse real, por eso esta página se centra en la experiencia, el lenguaje de movimiento y la marca.',
    browserDemo: 'Demo web',
    demoChooserTitle: 'Elige una ruta y mira cómo el cursor la recorre.',
    patternLabels: {
      Circle: 'Una órbita continua y suave.',
      Square: 'Lados rectos con giros definidos.',
      Triangle: 'Recorrido de tres lados con picos visibles.',
      'Figure 8': 'Un cruce en bucle con un tono más juguetón.',
      Parallelogram: 'Geometría inclinada con una postura más limpia.',
      Random: 'Puntos sueltos que aún se mantienen dentro del marco.',
    },
    ctaEyebrow: 'Entrega desktop',
    ctaTitle: 'El siguiente paso es una publicación real en GitHub para Windows.',
    ctaBody:
      'La forma más limpia de distribuir la app es con GitHub Releases. Así obtienes una página real de descarga sin inflar el historial del repositorio.',
    openReleases: 'Abrir releases',
    faqEyebrow: 'Preguntas',
    faqTitle: 'Preguntas comunes',
    faqs: [
      {
        question: '¿La web puede mover mi mouse real?',
        answer:
          'No. Los navegadores no pueden controlar el mouse del sistema. La web sirve para la marca, la historia del producto y una demo visual. La app de escritorio es la utilidad real.',
      },
      {
        question: '¿Qué patrones soporta WIGGLER?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram y Random están soportados en la app de escritorio y también aparecen en la demo del sitio.',
      },
      {
        question: '¿Cuál es la mejor forma de distribuir la app?',
        answer:
          'GitHub Releases es el lugar más limpio para la versión de Windows. Mantiene el ejecutable fuera del historial normal del repo y te da una página de descarga adecuada.',
      },
    ],
  },
  pt: {
    languageName: 'Português',
    navFeatures: 'Recursos',
    navDemo: 'Demo',
    navFaq: 'Perguntas',
    navGithub: 'GitHub',
    settings: 'Ajustes',
    theme: 'Tema',
    language: 'Idioma',
    light: 'Claro',
    dark: 'Escuro',
    heroEyebrow: 'Utilitário de desktop, agora com casa na web',
    heroTitle: 'Mantenha a sessão ativa sem brigar com a pessoa.',
    heroBody:
      'WIGGLER by SMBA é um app leve para Windows que espera o tempo ocioso e depois move o cursor em formas limpas e contidas até você tocar o mouse outra vez.',
    watchDemo: 'Ver demo',
    desktopDownloads: 'Downloads desktop',
    bestFor: 'Melhor para',
    bestForValue: 'Fluxos de trabalho no Windows',
    corePromise: 'Promessa central',
    corePromiseValue: 'Ocioso primeiro. Controle imediato.',
    status: 'Status',
    armed: 'Pronto',
    idleDelay: 'Espera ociosa',
    pattern: 'Padrão',
    tray: 'Bandeja',
    ready: 'Pronta',
    surfaceFootOne: 'O app desktop só assume depois da inatividade.',
    surfaceFootTwo: 'Seu mouse retoma o controle na hora.',
    featureEyebrow: 'Recurso',
    features: [
      {
        title: 'Controle por inatividade',
        body: 'WIGGLER fica armado em segundo plano e só começa a mover quando você realmente fica ocioso.',
      },
      {
        title: 'Retorno imediato',
        body: 'Assim que você toca o mouse, o app desktop para e devolve o controle imediatamente.',
      },
      {
        title: 'Movimento contido',
        body: 'Os padrões permanecem visíveis, dentro da tela e com intenção em vez de escapar pelas bordas.',
      },
    ],
    motionEyebrow: 'Biblioteca de movimento',
    motionTitle: 'Padrões visíveis, comportamento discreto.',
    motionBody:
      'O app desktop suporta seis estilos de movimento e permanece dentro dos limites da tela. O site espelha essa linguagem visual sem fingir controlar seu cursor real.',
    patternNames: {
      Circle: 'Círculo',
      Square: 'Quadrado',
      Triangle: 'Triângulo',
      'Figure 8': 'Figura 8',
      Parallelogram: 'Paralelogramo',
      Random: 'Aleatório',
    },
    demoEyebrow: 'Prévia interativa',
    demoTitle: 'Escolha a forma exata e veja o movimento.',
    demoBody:
      'Navegadores não podem mover seu mouse real, então esta página foca na experiência, na linguagem de movimento e na marca.',
    browserDemo: 'Demo web',
    demoChooserTitle: 'Escolha um trajeto e veja o cursor seguir o caminho.',
    patternLabels: {
      Circle: 'Uma órbita contínua e suave.',
      Square: 'Lados retos com curvas bem marcadas.',
      Triangle: 'Três lados com picos bem visíveis.',
      'Figure 8': 'Um cruzamento em loop com clima mais brincalhão.',
      Parallelogram: 'Geometria inclinada com uma postura mais limpa.',
      Random: 'Pontos livres que ainda ficam dentro da moldura.',
    },
    ctaEyebrow: 'Entrega desktop',
    ctaTitle: 'O próximo passo é uma release de verdade no GitHub para Windows.',
    ctaBody:
      'A forma mais limpa de distribuir o app é com GitHub Releases. Isso cria uma página real de download sem inflar o histórico do repositório.',
    openReleases: 'Abrir releases',
    faqEyebrow: 'Perguntas',
    faqTitle: 'Perguntas comuns',
    faqs: [
      {
        question: 'O site pode mover meu mouse real?',
        answer:
          'Não. Navegadores não podem controlar o mouse do sistema. O site existe para a marca, a história do produto e uma demo visual. O app desktop é a utilidade real.',
      },
      {
        question: 'Quais padrões o WIGGLER suporta?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram e Random existem no app desktop e também aparecem na demo do site.',
      },
      {
        question: 'Qual é a melhor forma de distribuir o app?',
        answer:
          'GitHub Releases é o lugar mais limpo para a versão de Windows. Mantém o executável fora do histórico normal do repo e oferece uma página de download adequada.',
      },
    ],
  },
  fr: {
    languageName: 'Français',
    navFeatures: 'Fonctions',
    navDemo: 'Démo',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    settings: 'Paramètres',
    theme: 'Thème',
    language: 'Langue',
    light: 'Clair',
    dark: 'Sombre',
    heroEyebrow: 'Utilitaire desktop, maintenant avec une maison sur le web',
    heroTitle: 'Gardez la session active sans lutter contre la personne.',
    heroBody:
      'WIGGLER by SMBA est une app Windows légère qui attend l’inactivité puis déplace le curseur selon des formes propres et limitées jusqu’au moment où vous touchez la souris.',
    watchDemo: 'Voir la démo',
    desktopDownloads: 'Téléchargements',
    bestFor: 'Idéal pour',
    bestForValue: 'Flux de travail Windows',
    corePromise: 'Promesse centrale',
    corePromiseValue: 'Inactif d’abord. Retour immédiat.',
    status: 'Statut',
    armed: 'Prêt',
    idleDelay: 'Délai d’inactivité',
    pattern: 'Motif',
    tray: 'Barre système',
    ready: 'Prêt',
    surfaceFootOne: 'L’app desktop prend le relais seulement après l’inactivité.',
    surfaceFootTwo: 'Votre souris reprend le contrôle instantanément.',
    featureEyebrow: 'Fonction',
    features: [
      {
        title: 'Contrôle basé sur l’inactivité',
        body: 'WIGGLER reste armé en arrière-plan et ne commence à bouger qu’après une vraie période d’inactivité.',
      },
      {
        title: 'Retour instantané',
        body: 'Dès que vous touchez la souris, l’app desktop s’arrête et vous rend le contrôle immédiatement.',
      },
      {
        title: 'Mouvement contenu',
        body: 'Les motifs restent visibles, dans l’écran et maîtrisés au lieu de partir au bord.',
      },
    ],
    motionEyebrow: 'Bibliothèque de mouvement',
    motionTitle: 'Motifs visibles, comportement discret.',
    motionBody:
      'L’app desktop propose six styles de mouvement et reste dans les limites de l’écran. Le site reprend ce langage visuel sans prétendre contrôler votre vrai curseur.',
    patternNames: {
      Circle: 'Cercle',
      Square: 'Carré',
      Triangle: 'Triangle',
      'Figure 8': 'Figure 8',
      Parallelogram: 'Parallélogramme',
      Random: 'Aléatoire',
    },
    demoEyebrow: 'Aperçu interactif',
    demoTitle: 'Choisissez la forme exacte et voyez le mouvement.',
    demoBody:
      'Les navigateurs ne peuvent pas déplacer votre vraie souris, donc cette page se concentre sur l’expérience, le langage de mouvement et la marque.',
    browserDemo: 'Démo web',
    demoChooserTitle: 'Choisissez un trajet et regardez le curseur le suivre.',
    patternLabels: {
      Circle: 'Une orbite continue et fluide.',
      Square: 'Des côtés droits avec des virages nets.',
      Triangle: 'Un trajet à trois côtés avec des pointes visibles.',
      'Figure 8': 'Une boucle croisée avec une sensation plus joueuse.',
      Parallelogram: 'Une géométrie décalée avec une allure plus nette.',
      Random: 'Des points libres qui restent tout de même encadrés.',
    },
    ctaEyebrow: 'Livraison desktop',
    ctaTitle: 'La prochaine étape est une vraie release GitHub pour la build Windows.',
    ctaBody:
      'La manière la plus propre de distribuer l’app passe par GitHub Releases. Vous obtenez ainsi une vraie page de téléchargement sans gonfler l’historique du repo.',
    openReleases: 'Ouvrir les releases',
    faqEyebrow: 'FAQ',
    faqTitle: 'Questions courantes',
    faqs: [
      {
        question: 'Le site peut-il déplacer ma vraie souris ?',
        answer:
          'Non. Les navigateurs ne peuvent pas contrôler la souris du système. Le site sert à la marque, à l’histoire du produit et à une démo visuelle. L’app desktop est l’outil réel.',
      },
      {
        question: 'Quels motifs WIGGLER prend-il en charge ?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram et Random sont pris en charge dans l’app desktop et apparaissent aussi dans la démo du site.',
      },
      {
        question: 'Quelle est la meilleure façon de distribuer l’app ?',
        answer:
          'GitHub Releases est l’endroit le plus propre pour la version Windows. Cela garde l’exécutable hors de l’historique normal du repo et fournit une vraie page de téléchargement.',
      },
    ],
  },
  de: {
    languageName: 'Deutsch',
    navFeatures: 'Funktionen',
    navDemo: 'Demo',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    settings: 'Einstellungen',
    theme: 'Design',
    language: 'Sprache',
    light: 'Hell',
    dark: 'Dunkel',
    heroEyebrow: 'Desktop-Tool, jetzt mit einer Heimat im Web',
    heroTitle: 'Halte die Sitzung aktiv, ohne gegen die Person zu arbeiten.',
    heroBody:
      'WIGGLER by SMBA ist eine leichte Windows-App, die auf Inaktivität wartet und dann den Cursor in sauberen, begrenzten Formen bewegt, bis du die Maus wieder anfasst.',
    watchDemo: 'Demo ansehen',
    desktopDownloads: 'Desktop-Downloads',
    bestFor: 'Am besten für',
    bestForValue: 'Windows-Workflows',
    corePromise: 'Kernversprechen',
    corePromiseValue: 'Erst inaktiv. Sofortige Übergabe.',
    status: 'Status',
    armed: 'Bereit',
    idleDelay: 'Leerlaufzeit',
    pattern: 'Muster',
    tray: 'Tray',
    ready: 'Bereit',
    surfaceFootOne: 'Die Desktop-App übernimmt erst nach echter Inaktivität.',
    surfaceFootTwo: 'Deine Maus gewinnt sofort zurück.',
    featureEyebrow: 'Funktion',
    features: [
      {
        title: 'Leerlauf zuerst',
        body: 'WIGGLER bleibt im Hintergrund scharf und startet nur dann, wenn du wirklich eine Weile inaktiv warst.',
      },
      {
        title: 'Sofortige Rückgabe',
        body: 'Sobald du die Maus bewegst, stoppt die Desktop-App und gibt dir die Kontrolle sofort zurück.',
      },
      {
        title: 'Begrenzte Bewegung',
        body: 'Die Muster bleiben sichtbar, auf dem Bildschirm und kontrolliert statt über den Rand hinauszudriften.',
      },
    ],
    motionEyebrow: 'Bewegungsbibliothek',
    motionTitle: 'Sichtbare Muster, ruhiges Verhalten.',
    motionBody:
      'Die Desktop-App unterstützt sechs Bewegungsarten und bleibt innerhalb der Bildschirmgrenzen. Die Website spiegelt diese Bewegungssprache, ohne vorzugeben, deinen echten Systemcursor zu steuern.',
    patternNames: {
      Circle: 'Kreis',
      Square: 'Quadrat',
      Triangle: 'Dreieck',
      'Figure 8': 'Acht',
      Parallelogram: 'Parallelogramm',
      Random: 'Zufall',
    },
    demoEyebrow: 'Interaktive Vorschau',
    demoTitle: 'Wähle die exakte Form und sieh die Bewegung.',
    demoBody:
      'Browser können deine echte Systemmaus nicht bewegen, deshalb konzentriert sich diese Seite auf Erlebnis, Bewegungssprache und Marke.',
    browserDemo: 'Browser-Demo',
    demoChooserTitle: 'Wähle einen Pfad und beobachte, wie der Cursor ihm folgt.',
    patternLabels: {
      Circle: 'Eine sanfte, durchgehende Umlaufbahn.',
      Square: 'Gerade Kanten mit klaren Wendungen.',
      Triangle: 'Dreiseitige Bewegung mit sichtbaren Spitzen.',
      'Figure 8': 'Eine gekreuzte Schleife mit verspielterem Gefühl.',
      Parallelogram: 'Versetzte Geometrie mit sauberer Diagonale.',
      Random: 'Freie Punkte, die trotzdem im Rahmen bleiben.',
    },
    ctaEyebrow: 'Desktop-Auslieferung',
    ctaTitle: 'Der nächste Schritt ist eine echte GitHub-Release für die Windows-Build.',
    ctaBody:
      'Der sauberste Weg zur Verteilung der App ist GitHub Releases. Das gibt dir eine echte Download-Seite, ohne das Repo mit einer großen EXE aufzublähen.',
    openReleases: 'Releases öffnen',
    faqEyebrow: 'FAQ',
    faqTitle: 'Häufige Fragen',
    faqs: [
      {
        question: 'Kann die Website meine echte Maus bewegen?',
        answer:
          'Nein. Browser dürfen die Systemmaus nicht steuern. Die Website ist für Marke, Produktgeschichte und eine Live-Demo gedacht. Die Desktop-App ist das echte Tool.',
      },
      {
        question: 'Welche Muster unterstützt WIGGLER?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram und Random werden in der Desktop-App unterstützt und erscheinen auch in der Website-Demo.',
      },
      {
        question: 'Wie verteilt man die App am besten?',
        answer:
          'GitHub Releases ist der sauberste Ort für die Windows-Version. So bleibt die EXE aus der normalen Repo-Historie heraus und du bekommst eine richtige Download-Seite.',
      },
    ],
  },
  it: {
    languageName: 'Italiano',
    navFeatures: 'Funzioni',
    navDemo: 'Demo',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    settings: 'Impostazioni',
    theme: 'Tema',
    language: 'Lingua',
    light: 'Chiaro',
    dark: 'Scuro',
    heroEyebrow: 'Utility desktop, ora con una casa sul web',
    heroTitle: 'Mantieni attiva la sessione senza lottare con la persona.',
    heroBody:
      'WIGGLER by SMBA è una leggera app Windows che aspetta l’inattività e poi muove il cursore in forme pulite e contenute finché non tocchi di nuovo il mouse.',
    watchDemo: 'Guarda la demo',
    desktopDownloads: 'Download desktop',
    bestFor: 'Ideale per',
    bestForValue: 'Flussi di lavoro Windows',
    corePromise: 'Promessa centrale',
    corePromiseValue: 'Prima inattivo. Ritorno immediato.',
    status: 'Stato',
    armed: 'Pronto',
    idleDelay: 'Attesa inattiva',
    pattern: 'Schema',
    tray: 'Tray',
    ready: 'Pronto',
    surfaceFootOne: 'L’app desktop prende il controllo solo dopo l’inattività.',
    surfaceFootTwo: 'Il tuo mouse riprende subito il controllo.',
    featureEyebrow: 'Funzione',
    features: [
      {
        title: 'Controllo basato sull’inattività',
        body: 'WIGGLER resta armato in background e inizia a muoversi solo dopo una vera fase di inattività.',
      },
      {
        title: 'Ritorno immediato',
        body: 'Appena tocchi il mouse, l’app desktop si ferma e ti restituisce il controllo all’istante.',
      },
      {
        title: 'Movimento contenuto',
        body: 'Gli schemi restano visibili, sullo schermo e controllati invece di scivolare fuori bordo.',
      },
    ],
    motionEyebrow: 'Libreria di movimento',
    motionTitle: 'Schemi visibili, comportamento discreto.',
    motionBody:
      'L’app desktop supporta sei stili di movimento e resta entro i limiti dello schermo. Il sito riflette questo linguaggio visivo senza fingere di controllare il tuo cursore reale.',
    patternNames: {
      Circle: 'Cerchio',
      Square: 'Quadrato',
      Triangle: 'Triangolo',
      'Figure 8': 'Figura 8',
      Parallelogram: 'Parallelogramma',
      Random: 'Casuale',
    },
    demoEyebrow: 'Anteprima interattiva',
    demoTitle: 'Scegli la forma esatta e guarda il movimento.',
    demoBody:
      'I browser non possono muovere il tuo vero mouse di sistema, quindi questa pagina si concentra su esperienza, linguaggio del movimento e brand.',
    browserDemo: 'Demo web',
    demoChooserTitle: 'Scegli un percorso e guarda il cursore seguirlo.',
    patternLabels: {
      Circle: 'Un’orbita continua e fluida.',
      Square: 'Lati dritti con svolte nette.',
      Triangle: 'Tre lati con picchi ben visibili.',
      'Figure 8': 'Un incrocio in loop con un tono più giocoso.',
      Parallelogram: 'Geometria inclinata con una postura più pulita.',
      Random: 'Punti liberi che restano comunque entro i limiti.',
    },
    ctaEyebrow: 'Consegna desktop',
    ctaTitle: 'Il prossimo passo è una vera release GitHub per la build Windows.',
    ctaBody:
      'Il modo più pulito per distribuire l’app è usare GitHub Releases. Così ottieni una vera pagina di download senza gonfiare la cronologia del repo.',
    openReleases: 'Apri releases',
    faqEyebrow: 'FAQ',
    faqTitle: 'Domande comuni',
    faqs: [
      {
        question: 'Il sito può muovere il mio vero mouse?',
        answer:
          'No. I browser non possono controllare il mouse del sistema. Il sito serve per il brand, la storia del prodotto e una demo visiva. L’app desktop è l’utilità reale.',
      },
      {
        question: 'Quali schemi supporta WIGGLER?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram e Random sono supportati nell’app desktop e compaiono anche nella demo del sito.',
      },
      {
        question: 'Qual è il modo migliore per distribuire l’app?',
        answer:
          'GitHub Releases è il posto più pulito per la versione Windows. Tiene l’eseguibile fuori dalla cronologia normale del repo e ti dà una vera pagina di download.',
      },
    ],
  },
  nl: {
    languageName: 'Nederlands',
    navFeatures: 'Functies',
    navDemo: 'Demo',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    settings: 'Instellingen',
    theme: 'Thema',
    language: 'Taal',
    light: 'Licht',
    dark: 'Donker',
    heroEyebrow: 'Desktoptool, nu met een thuis op het web',
    heroTitle: 'Houd de sessie actief zonder tegen de gebruiker in te werken.',
    heroBody:
      'WIGGLER by SMBA is een lichte Windows-app die wacht op inactiviteit en daarna de cursor in strakke, begrensde vormen beweegt tot je de muis weer aanraakt.',
    watchDemo: 'Bekijk demo',
    desktopDownloads: 'Desktopdownloads',
    bestFor: 'Het beste voor',
    bestForValue: 'Windows-workflows',
    corePromise: 'Kernbelofte',
    corePromiseValue: 'Eerst inactief. Meteen teruggeven.',
    status: 'Status',
    armed: 'Gereed',
    idleDelay: 'Wachttijd',
    pattern: 'Patroon',
    tray: 'Systeemvak',
    ready: 'Gereed',
    surfaceFootOne: 'De desktop-app neemt pas over na echte inactiviteit.',
    surfaceFootTwo: 'Jouw muis wint meteen terug.',
    featureEyebrow: 'Functie',
    features: [
      {
        title: 'Inactiviteit eerst',
        body: 'WIGGLER blijft op de achtergrond klaarstaan en begint pas te bewegen wanneer je echt even inactief bent geweest.',
      },
      {
        title: 'Directe overdracht',
        body: 'Zodra je de muis beweegt, stopt de desktop-app en krijg je direct weer controle.',
      },
      {
        title: 'Begrensde beweging',
        body: 'Patronen blijven zichtbaar, op het scherm en beheerst in plaats van van de rand af te drijven.',
      },
    ],
    motionEyebrow: 'Bewegingsbibliotheek',
    motionTitle: 'Zichtbare patronen, rustig gedrag.',
    motionBody:
      'De desktop-app ondersteunt zes bewegingsstijlen en blijft binnen de schermgrenzen. De site weerspiegelt die bewegingstaal zonder te doen alsof hij je echte cursor kan besturen.',
    patternNames: {
      Circle: 'Cirkel',
      Square: 'Vierkant',
      Triangle: 'Driehoek',
      'Figure 8': 'Figuur 8',
      Parallelogram: 'Parallellogram',
      Random: 'Willekeurig',
    },
    demoEyebrow: 'Interactieve preview',
    demoTitle: 'Kies de exacte vorm en bekijk de beweging.',
    demoBody:
      'Browsers kunnen je echte systeemmuis niet bewegen, dus deze pagina draait om beleving, bewegingstaal en merkgevoel.',
    browserDemo: 'Browserdemo',
    demoChooserTitle: 'Kies een pad en kijk hoe de cursor het volgt.',
    patternLabels: {
      Circle: 'Een vloeiende, doorlopende baan.',
      Square: 'Rechte zijden met heldere hoeken.',
      Triangle: 'Een drieslag met duidelijke pieken.',
      'Figure 8': 'Een gekruiste lus met een speelser gevoel.',
      Parallelogram: 'Schuine geometrie met een strakkere houding.',
      Random: 'Vrije punten die toch binnen het kader blijven.',
    },
    ctaEyebrow: 'Desktopdistributie',
    ctaTitle: 'De volgende stap is een echte GitHub-release voor de Windows-build.',
    ctaBody:
      'De netste manier om de app te verspreiden is via GitHub Releases. Zo krijg je een echte downloadpagina zonder de repogeschiedenis op te blazen met een grote EXE.',
    openReleases: 'Open releases',
    faqEyebrow: 'FAQ',
    faqTitle: 'Veelgestelde vragen',
    faqs: [
      {
        question: 'Kan de website mijn echte muis bewegen?',
        answer:
          'Nee. Browsers mogen de systeemmuis niet besturen. De website is er voor merk, productverhaal en een live demo. De desktop-app is het echte hulpprogramma.',
      },
      {
        question: 'Welke patronen ondersteunt WIGGLER?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram en Random worden ondersteund in de desktop-app en verschijnen ook in de sitedemo.',
      },
      {
        question: 'Wat is de beste manier om de app te verspreiden?',
        answer:
          'GitHub Releases is de schoonste plek voor de Windows-versie. Het houdt het uitvoerbare bestand uit de normale repogeschiedenis en geeft je een echte downloadpagina.',
      },
    ],
  },
  sv: {
    languageName: 'Svenska',
    navFeatures: 'Funktioner',
    navDemo: 'Demo',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    settings: 'Inställningar',
    theme: 'Tema',
    language: 'Språk',
    light: 'Ljust',
    dark: 'Mörkt',
    heroEyebrow: 'Desktopverktyg, nu med ett hem på webben',
    heroTitle: 'Håll sessionen aktiv utan att kämpa mot användaren.',
    heroBody:
      'WIGGLER by SMBA är en lätt Windows-app som väntar på inaktivitet och sedan flyttar markören i rena, begränsade former tills du rör musen igen.',
    watchDemo: 'Se demo',
    desktopDownloads: 'Desktopnedladdningar',
    bestFor: 'Bäst för',
    bestForValue: 'Windows-arbetsflöden',
    corePromise: 'Kärnlöfte',
    corePromiseValue: 'Först inaktiv. Omedelbar återlämning.',
    status: 'Status',
    armed: 'Redo',
    idleDelay: 'Vilofördröjning',
    pattern: 'Mönster',
    tray: 'Fack',
    ready: 'Redo',
    surfaceFootOne: 'Desktopappen tar bara över efter verklig inaktivitet.',
    surfaceFootTwo: 'Din mus vinner tillbaka kontrollen direkt.',
    featureEyebrow: 'Funktion',
    features: [
      {
        title: 'Inaktivitet först',
        body: 'WIGGLER står redo i bakgrunden och börjar bara röra sig när du faktiskt har varit inaktiv ett tag.',
      },
      {
        title: 'Omedelbar återgång',
        body: 'Så fort du rör musen stannar desktopappen och lämnar tillbaka kontrollen direkt.',
      },
      {
        title: 'Begränsad rörelse',
        body: 'Mönstren förblir synliga, inom skärmen och kontrollerade i stället för att glida iväg vid kanten.',
      },
    ],
    motionEyebrow: 'Rörelsebibliotek',
    motionTitle: 'Synliga mönster, lugnt beteende.',
    motionBody:
      'Desktopappen stöder sex rörelsestilar och håller sig inom skärmens gränser. Webbplatsen speglar det rörelsespråket utan att låtsas styra din riktiga markör.',
    patternNames: {
      Circle: 'Cirkel',
      Square: 'Kvadrat',
      Triangle: 'Triangel',
      'Figure 8': 'Figur 8',
      Parallelogram: 'Parallellogram',
      Random: 'Slumpmässig',
    },
    demoEyebrow: 'Interaktiv förhandsvisning',
    demoTitle: 'Välj exakt form och se rörelsen.',
    demoBody:
      'Webbläsare kan inte flytta din riktiga systemmus, så den här sidan fokuserar på upplevelsen, rörelsespråket och varumärket.',
    browserDemo: 'Webbdemo',
    demoChooserTitle: 'Välj en bana och se markören följa den.',
    patternLabels: {
      Circle: 'En mjuk och obruten bana.',
      Square: 'Raka sidor med tydliga svängar.',
      Triangle: 'Tre sidor med tydliga toppar.',
      'Figure 8': 'En korsande loop med en mer lekfull känsla.',
      Parallelogram: 'Förskjuten geometri med renare diagonal känsla.',
      Random: 'Fria punkter som ändå håller sig inom ramen.',
    },
    ctaEyebrow: 'Desktopleverans',
    ctaTitle: 'Nästa steg är en riktig GitHub-release för Windows-versionen.',
    ctaBody:
      'Det renaste sättet att distribuera appen är via GitHub Releases. Då får du en riktig nedladdningssida utan att blåsa upp repots historik med en stor EXE.',
    openReleases: 'Öppna releases',
    faqEyebrow: 'FAQ',
    faqTitle: 'Vanliga frågor',
    faqs: [
      {
        question: 'Kan webbplatsen flytta min riktiga mus?',
        answer:
          'Nej. Webbläsare får inte styra systemmusen. Webbplatsen finns för varumärket, produktberättelsen och en visuell demo. Desktopappen är det riktiga verktyget.',
      },
      {
        question: 'Vilka mönster stöder WIGGLER?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram och Random stöds i desktopappen och visas också i webbplatsens demo.',
      },
      {
        question: 'Vad är det bästa sättet att distribuera appen?',
        answer:
          'GitHub Releases är den renaste platsen för Windows-versionen. Det håller körbara filer borta från den normala repohistoriken och ger dig en riktig nedladdningssida.',
      },
    ],
  },
  ja: {
    languageName: '日本語',
    navFeatures: '機能',
    navDemo: 'デモ',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    settings: '設定',
    theme: 'テーマ',
    language: '言語',
    light: 'ライト',
    dark: 'ダーク',
    heroEyebrow: 'デスクトップユーティリティ、いまはウェブにも展開',
    heroTitle: 'ユーザーとぶつからずにセッションを維持します。',
    heroBody:
      'WIGGLER by SMBA は、一定時間の無操作を待ってから、マウスをきれいで制御された形で動かし、ユーザーが触れた瞬間に止まる軽量な Windows アプリです。',
    watchDemo: 'デモを見る',
    desktopDownloads: 'デスクトップ版のダウンロード',
    bestFor: '最適な用途',
    bestForValue: 'Windows の作業フロー',
    corePromise: 'コアの約束',
    corePromiseValue: '無操作を優先。即座に返す。',
    status: '状態',
    armed: '待機中',
    idleDelay: '待機時間',
    pattern: 'パターン',
    tray: 'トレイ',
    ready: '準備完了',
    surfaceFootOne: 'デスクトップ版は無操作になってからだけ動作します。',
    surfaceFootTwo: 'マウスを動かせばすぐにユーザーへ戻ります。',
    featureEyebrow: '機能',
    features: [
      {
        title: '無操作優先の制御',
        body: 'WIGGLER はバックグラウンドで待機し、本当に操作が止まったときだけ動き始めます。',
      },
      {
        title: '即時の返却',
        body: 'ユーザーがマウスを触れた瞬間にデスクトップ版は停止し、コントロールをすぐに返します。',
      },
      {
        title: '画面内に収まる動き',
        body: 'パターンは見える範囲にとどまり、画面の外へ流れていきません。',
      },
    ],
    motionEyebrow: 'モーションライブラリ',
    motionTitle: '見える動き、静かなふるまい。',
    motionBody:
      'デスクトップ版は 6 種類の動きを持ち、画面の範囲内で移動します。このサイトはその動きの言語を表現しますが、実際のシステムマウスは制御しません。',
    patternNames: {
      Circle: '円',
      Square: '四角',
      Triangle: '三角',
      'Figure 8': '8の字',
      Parallelogram: '平行四辺形',
      Random: 'ランダム',
    },
    demoEyebrow: 'インタラクティブプレビュー',
    demoTitle: '形を選んで動きを確認できます。',
    demoBody:
      'ブラウザは実際のシステムマウスを動かせないため、このページは体験、動きの言語、ブランド表現に集中しています。',
    browserDemo: 'ブラウザデモ',
    demoChooserTitle: 'パスを選んで、カーソルがその通りに動く様子を見てください。',
    patternLabels: {
      Circle: 'なめらかで連続した円運動です。',
      Square: '直線の辺ときれいな角で構成されます。',
      Triangle: '三辺をたどる動きで、頂点がはっきり見えます。',
      'Figure 8': '少し遊び心のある 8 の字ループです。',
      Parallelogram: '斜めの印象がある、整った幾何学です。',
      Random: '自由に見えても、ちゃんと枠内に収まります。',
    },
    ctaEyebrow: 'デスクトップ配布',
    ctaTitle: '次のステップは、Windows ビルドを GitHub Release として公開することです。',
    ctaBody:
      'アプリを配布する最もきれいな方法は GitHub Releases です。大きな実行ファイルでリポジトリ履歴を重くせず、専用のダウンロードページを作れます。',
    openReleases: 'Releases を開く',
    faqEyebrow: 'FAQ',
    faqTitle: 'よくある質問',
    faqs: [
      {
        question: 'このサイトで本当にマウスを動かせますか？',
        answer:
          'いいえ。ブラウザはシステムマウスを制御できません。このサイトはブランド紹介、製品説明、そして視覚的なデモのためのものです。実際の機能はデスクトップ版にあります。',
      },
      {
        question: 'WIGGLER はどのパターンに対応していますか？',
        answer:
          'Circle、Square、Triangle、Figure 8、Parallelogram、Random に対応しており、サイトのデモでも同じ流れを確認できます。',
      },
      {
        question: 'アプリを配布するにはどうするのが一番ですか？',
        answer:
          'Windows 版の配布には GitHub Releases が最適です。実行ファイルを通常の履歴から分離し、きれいなダウンロードページを用意できます。',
      },
    ],
  },
  ko: {
    languageName: '한국어',
    navFeatures: '기능',
    navDemo: '데모',
    navFaq: 'FAQ',
    navGithub: 'GitHub',
    settings: '설정',
    theme: '테마',
    language: '언어',
    light: '라이트',
    dark: '다크',
    heroEyebrow: '데스크톱 유틸리티, 이제 웹에서도 소개됩니다',
    heroTitle: '사용자와 싸우지 않고 세션을 유지합니다.',
    heroBody:
      'WIGGLER by SMBA 는 일정 시간 동안 입력이 없을 때만 커서를 깨끗하고 제한된 경로로 움직이고, 사용자가 마우스를 건드리는 순간 즉시 멈추는 가벼운 Windows 앱입니다.',
    watchDemo: '데모 보기',
    desktopDownloads: '데스크톱 다운로드',
    bestFor: '적합한 대상',
    bestForValue: 'Windows 작업 흐름',
    corePromise: '핵심 약속',
    corePromiseValue: '유휴 우선. 즉시 반환.',
    status: '상태',
    armed: '대기 중',
    idleDelay: '유휴 대기',
    pattern: '패턴',
    tray: '트레이',
    ready: '준비 완료',
    surfaceFootOne: '데스크톱 앱은 실제로 유휴 상태일 때만 동작합니다.',
    surfaceFootTwo: '마우스를 움직이면 즉시 사용자에게 돌아갑니다.',
    featureEyebrow: '기능',
    features: [
      {
        title: '유휴 우선 제어',
        body: 'WIGGLER 는 백그라운드에서 대기하다가 사용자가 정말로 멈춰 있을 때만 움직이기 시작합니다.',
      },
      {
        title: '즉시 반환',
        body: '사용자가 마우스를 건드리는 순간 데스크톱 앱은 멈추고 제어권을 바로 돌려줍니다.',
      },
      {
        title: '화면 안에 머무는 이동',
        body: '패턴은 화면 안에서 보이도록 유지되며 가장자리 밖으로 흘러가지 않습니다.',
      },
    ],
    motionEyebrow: '모션 라이브러리',
    motionTitle: '눈에 보이는 패턴, 조용한 동작.',
    motionBody:
      '데스크톱 앱은 여섯 가지 움직임을 지원하고 화면 경계 안에서만 이동합니다. 이 사이트는 그 움직임 언어를 보여주지만 실제 시스템 마우스를 제어하지는 않습니다.',
    patternNames: {
      Circle: '원',
      Square: '사각형',
      Triangle: '삼각형',
      'Figure 8': '8자',
      Parallelogram: '평행사변형',
      Random: '랜덤',
    },
    demoEyebrow: '인터랙티브 미리보기',
    demoTitle: '정확한 모양을 선택해 움직임을 볼 수 있습니다.',
    demoBody:
      '브라우저는 실제 시스템 마우스를 움직일 수 없기 때문에, 이 페이지는 경험, 움직임 언어, 브랜드 표현에 집중합니다.',
    browserDemo: '브라우저 데모',
    demoChooserTitle: '경로를 선택하고 커서가 그대로 따라가는 모습을 보세요.',
    patternLabels: {
      Circle: '부드럽고 끊기지 않는 원형 이동입니다.',
      Square: '직선 변과 또렷한 회전이 보입니다.',
      Triangle: '세 변을 따라가며 꼭짓점이 분명하게 보입니다.',
      'Figure 8': '조금 더 유쾌한 느낌의 8자 루프입니다.',
      Parallelogram: '사선 감각이 살아 있는 정돈된 도형입니다.',
      Random: '자유롭게 보이지만 여전히 범위 안에 머뭅니다.',
    },
    ctaEyebrow: '데스크톱 배포',
    ctaTitle: '다음 단계는 Windows 빌드를 GitHub Release 로 공개하는 것입니다.',
    ctaBody:
      '앱을 배포하는 가장 깔끔한 방법은 GitHub Releases 입니다. 큰 실행 파일로 저장소 이력을 무겁게 만들지 않으면서 전용 다운로드 페이지를 만들 수 있습니다.',
    openReleases: 'Releases 열기',
    faqEyebrow: 'FAQ',
    faqTitle: '자주 묻는 질문',
    faqs: [
      {
        question: '이 웹사이트가 실제 마우스를 움직일 수 있나요?',
        answer:
          '아니요. 브라우저는 시스템 마우스를 제어할 수 없습니다. 이 사이트는 브랜드, 제품 설명, 시각적 데모를 위한 공간입니다. 실제 기능은 데스크톱 앱에 있습니다.',
      },
      {
        question: 'WIGGLER 는 어떤 패턴을 지원하나요?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram, Random 을 지원하며, 사이트 데모에서도 같은 흐름을 볼 수 있습니다.',
      },
      {
        question: '앱을 배포하는 가장 좋은 방법은 무엇인가요?',
        answer:
          'Windows 버전 배포에는 GitHub Releases 가 가장 깔끔합니다. 실행 파일을 일반 저장소 이력과 분리하고 전용 다운로드 페이지를 제공할 수 있습니다.',
      },
    ],
  },
  zh: {
    languageName: '中文',
    navFeatures: '功能',
    navDemo: '演示',
    navFaq: '常见问题',
    navGithub: 'GitHub',
    settings: '设置',
    theme: '主题',
    language: '语言',
    light: '浅色',
    dark: '深色',
    heroEyebrow: '桌面工具，现在也有网页主页',
    heroTitle: '保持会话活跃，同时不和用户抢控制权。',
    heroBody:
      'WIGGLER by SMBA 是一个轻量级 Windows 应用，只会在用户空闲一段时间后按受控路径移动鼠标，并且用户一碰鼠标就会立刻停止。',
    watchDemo: '查看演示',
    desktopDownloads: '桌面版下载',
    bestFor: '适合',
    bestForValue: 'Windows 工作流',
    corePromise: '核心承诺',
    corePromiseValue: '先等待空闲，立刻交还控制。',
    status: '状态',
    armed: '待命',
    idleDelay: '空闲延迟',
    pattern: '图形',
    tray: '托盘',
    ready: '已就绪',
    surfaceFootOne: '桌面版只会在真正空闲后接管。',
    surfaceFootTwo: '用户一动鼠标就会马上停止。',
    featureEyebrow: '功能',
    features: [
      {
        title: '空闲优先控制',
        body: 'WIGGLER 在后台待命，只有当用户真的一段时间没有操作时才开始移动。',
      },
      {
        title: '即时交还',
        body: '用户一碰鼠标，桌面应用就会停止，并且立刻把控制权交还回来。',
      },
      {
        title: '受限移动',
        body: '图形轨迹会保持可见、留在屏幕内，而且不会漂出边界。',
      },
    ],
    motionEyebrow: '动作库',
    motionTitle: '图形可见，行为安静。',
    motionBody:
      '桌面版支持六种移动模式，并始终保持在屏幕范围内。网站会展示这种运动语言，但不会假装控制你的真实系统鼠标。',
    patternNames: {
      Circle: '圆形',
      Square: '方形',
      Triangle: '三角形',
      'Figure 8': '8字形',
      Parallelogram: '平行四边形',
      Random: '随机',
    },
    demoEyebrow: '交互预览',
    demoTitle: '选择具体图形并预览动作。',
    demoBody:
      '浏览器不能移动你的真实系统鼠标，所以这个页面更关注体验、动作语言和品牌呈现。',
    browserDemo: '网页演示',
    demoChooserTitle: '选择一条路径，观看光标沿着它移动。',
    patternLabels: {
      Circle: '平滑、连续的圆形轨道。',
      Square: '直边配合干净利落的转角。',
      Triangle: '三边路径，顶点清晰可见。',
      'Figure 8': '更有玩味感的 8 字循环。',
      Parallelogram: '带斜向气质的整洁几何图形。',
      Random: '虽然自由，但依然保持在边界之内。',
    },
    ctaEyebrow: '桌面版交付',
    ctaTitle: '下一步是把 Windows 构建正式发布到 GitHub Releases。',
    ctaBody:
      '分发这款应用最干净的方式就是 GitHub Releases。这样既能得到真正的下载页面，也不会把大型可执行文件塞进仓库历史中。',
    openReleases: '打开 Releases',
    faqEyebrow: '常见问题',
    faqTitle: '常见问题',
    faqs: [
      {
        question: '这个网站能移动我的真实鼠标吗？',
        answer:
          '不能。浏览器无法控制系统鼠标。这个网站用于品牌展示、产品介绍和可视化演示。真正的功能在桌面应用中。',
      },
      {
        question: 'WIGGLER 支持哪些图形模式？',
        answer:
          '支持 Circle、Square、Triangle、Figure 8、Parallelogram 和 Random，这些模式也都体现在网站演示中。',
      },
      {
        question: '发布这个应用最好的方式是什么？',
        answer:
          'GitHub Releases 是发布 Windows 版本最干净的地方。它能让可执行文件脱离普通仓库历史，并提供专门的下载页面。',
      },
    ],
  },
  ar: {
    languageName: 'العربية',
    navFeatures: 'الميزات',
    navDemo: 'العرض',
    navFaq: 'الأسئلة',
    navGithub: 'GitHub',
    settings: 'الإعدادات',
    theme: 'السمة',
    language: 'اللغة',
    light: 'فاتح',
    dark: 'داكن',
    heroEyebrow: 'أداة سطح مكتب، والآن لها حضور على الويب',
    heroTitle: 'تحافظ على الجلسة نشطة من دون أن تتصارع مع المستخدم.',
    heroBody:
      'WIGGLER by SMBA هو تطبيق Windows خفيف ينتظر حتى يصبح المستخدم غير نشط، ثم يحرك المؤشر في مسارات نظيفة ومحددة إلى أن يلمس المستخدم الفأرة مرة أخرى.',
    watchDemo: 'شاهد العرض',
    desktopDownloads: 'تنزيلات سطح المكتب',
    bestFor: 'الأفضل لـ',
    bestForValue: 'سير العمل على Windows',
    corePromise: 'الوعد الأساسي',
    corePromiseValue: 'ينتظر الخمول أولاً. ويعيد التحكم فوراً.',
    status: 'الحالة',
    armed: 'جاهز',
    idleDelay: 'مهلة الخمول',
    pattern: 'النمط',
    tray: 'الدرج',
    ready: 'جاهز',
    surfaceFootOne: 'تطبيق سطح المكتب يتدخل فقط بعد الخمول الحقيقي.',
    surfaceFootTwo: 'الفأرة تعود للمستخدم فوراً عند التحريك.',
    featureEyebrow: 'ميزة',
    features: [
      {
        title: 'تحكم يعتمد على الخمول',
        body: 'يبقى WIGGLER في الخلفية ولا يبدأ الحركة إلا عندما يكون المستخدم متوقفاً فعلاً عن التحريك.',
      },
      {
        title: 'استعادة فورية',
        body: 'بمجرد أن يلمس المستخدم الفأرة، يتوقف التطبيق ويعيد التحكم فوراً.',
      },
      {
        title: 'حركة ضمن الحدود',
        body: 'تبقى الأنماط مرئية وداخل الشاشة وهادئة بدلاً من الانجراف خارج الحواف.',
      },
    ],
    motionEyebrow: 'مكتبة الحركة',
    motionTitle: 'أنماط واضحة وسلوك هادئ.',
    motionBody:
      'يدعم تطبيق سطح المكتب ستة أساليب للحركة ويبقى ضمن حدود الشاشة. يعكس الموقع هذه اللغة البصرية من دون الادعاء بأنه يتحكم في مؤشر النظام الحقيقي.',
    patternNames: {
      Circle: 'دائرة',
      Square: 'مربع',
      Triangle: 'مثلث',
      'Figure 8': 'رقم 8',
      Parallelogram: 'متوازي أضلاع',
      Random: 'عشوائي',
    },
    demoEyebrow: 'معاينة تفاعلية',
    demoTitle: 'اختر الشكل الدقيق وشاهد الحركة.',
    demoBody:
      'المتصفحات لا تستطيع تحريك فأرتك الحقيقية، لذلك تركز هذه الصفحة على التجربة ولغة الحركة والهوية.',
    browserDemo: 'عرض المتصفح',
    demoChooserTitle: 'اختر مساراً وشاهد المؤشر يتبعه.',
    patternLabels: {
      Circle: 'مدار دائري ناعم ومتصل.',
      Square: 'حواف مستقيمة مع انعطافات واضحة.',
      Triangle: 'مسار ثلاثي الجوانب مع قمم ظاهرة.',
      'Figure 8': 'حلقة متقاطعة بإحساس أكثر مرحاً.',
      Parallelogram: 'هندسة مائلة بإيقاع أنظف.',
      Random: 'نقاط حرة لكنها تبقى داخل الإطار.',
    },
    ctaEyebrow: 'توزيع سطح المكتب',
    ctaTitle: 'الخطوة التالية هي نشر إصدار Windows الأول عبر GitHub Releases.',
    ctaBody:
      'أنظف طريقة لتوزيع التطبيق هي GitHub Releases. فهي تمنحك صفحة تنزيل حقيقية من دون تضخيم تاريخ المستودع بملف تنفيذي كبير.',
    openReleases: 'افتح Releases',
    faqEyebrow: 'الأسئلة',
    faqTitle: 'أسئلة شائعة',
    faqs: [
      {
        question: 'هل يستطيع الموقع تحريك فأرتي الحقيقية؟',
        answer:
          'لا. المتصفحات لا يمكنها التحكم في فأرة النظام. هذا الموقع مخصص للهوية وقصة المنتج والعرض المرئي. أما الأداة الحقيقية فهي تطبيق سطح المكتب.',
      },
      {
        question: 'ما الأنماط التي يدعمها WIGGLER؟',
        answer:
          'يدعم Circle وSquare وTriangle وFigure 8 وParallelogram وRandom، وكلها تظهر أيضاً في عرض الموقع.',
      },
      {
        question: 'ما أفضل طريقة لنشر التطبيق؟',
        answer:
          'يعد GitHub Releases أنظف مكان لإصدار Windows. فهو يبقي الملف التنفيذي خارج التاريخ العادي للمستودع ويعطيك صفحة تنزيل مناسبة.',
      },
    ],
  },
  hi: {
    languageName: 'हिन्दी',
    navFeatures: 'फीचर्स',
    navDemo: 'डेमो',
    navFaq: 'सवाल',
    navGithub: 'GitHub',
    settings: 'सेटिंग्स',
    theme: 'थीम',
    language: 'भाषा',
    light: 'लाइट',
    dark: 'डार्क',
    heroEyebrow: 'डेस्कटॉप यूटिलिटी, अब वेब पर भी मौजूद',
    heroTitle: 'सेशन को सक्रिय रखता है, बिना यूज़र से लड़ाई किए.',
    heroBody:
      'WIGGLER by SMBA एक हल्का Windows ऐप है जो पहले निष्क्रियता का इंतजार करता है, फिर कर्सर को साफ और सीमित आकारों में चलाता है, और यूज़र के छूते ही तुरंत रुक जाता है.',
    watchDemo: 'डेमो देखें',
    desktopDownloads: 'डेस्कटॉप डाउनलोड',
    bestFor: 'सबसे बेहतर',
    bestForValue: 'Windows वर्कफ़्लो',
    corePromise: 'मुख्य वादा',
    corePromiseValue: 'पहले निष्क्रियता। तुरंत वापसी.',
    status: 'स्थिति',
    armed: 'तैयार',
    idleDelay: 'निष्क्रिय देरी',
    pattern: 'पैटर्न',
    tray: 'ट्रे',
    ready: 'तैयार',
    surfaceFootOne: 'डेस्कटॉप ऐप केवल वास्तविक निष्क्रियता के बाद ही नियंत्रण लेता है.',
    surfaceFootTwo: 'माउस छूते ही नियंत्रण तुरंत यूज़र को लौट जाता है.',
    featureEyebrow: 'फ़ीचर',
    features: [
      {
        title: 'निष्क्रियता-आधारित नियंत्रण',
        body: 'WIGGLER बैकग्राउंड में तैयार रहता है और तभी चलना शुरू करता है जब यूज़र सच में कुछ समय तक निष्क्रिय हो.',
      },
      {
        title: 'तुरंत वापसी',
        body: 'जैसे ही यूज़र माउस हिलाता है, डेस्कटॉप ऐप रुक जाता है और नियंत्रण तुरंत वापस देता है.',
      },
      {
        title: 'सीमित मूवमेंट',
        body: 'पैटर्न स्क्रीन के अंदर, दिखने लायक और नियंत्रित रहते हैं, किनारे से बाहर नहीं बहते.',
      },
    ],
    motionEyebrow: 'मोशन लाइब्रेरी',
    motionTitle: 'दिखने वाले पैटर्न, शांत व्यवहार.',
    motionBody:
      'डेस्कटॉप ऐप छह तरह की मूवमेंट स्टाइल को सपोर्ट करता है और स्क्रीन की सीमा के भीतर रहता है. वेबसाइट उसी मूवमेंट भाषा को दिखाती है, लेकिन आपके असली सिस्टम माउस को नियंत्रित नहीं करती.',
    patternNames: {
      Circle: 'सर्कल',
      Square: 'स्क्वेयर',
      Triangle: 'ट्रायंगल',
      'Figure 8': 'फिगर 8',
      Parallelogram: 'पैरललोग्राम',
      Random: 'रैंडम',
    },
    demoEyebrow: 'इंटरैक्टिव प्रीव्यू',
    demoTitle: 'सही आकार चुनें और मूवमेंट देखें.',
    demoBody:
      'ब्राउज़र आपके असली सिस्टम माउस को नहीं चला सकते, इसलिए यह पेज अनुभव, मूवमेंट भाषा और ब्रांड प्रस्तुति पर फोकस करता है.',
    browserDemo: 'ब्राउज़र डेमो',
    demoChooserTitle: 'एक रास्ता चुनें और देखें कि कर्सर उसे कैसे फॉलो करता है.',
    patternLabels: {
      Circle: 'एक स्मूद और लगातार गोल पथ.',
      Square: 'सीधी भुजाएँ और साफ मोड़.',
      Triangle: 'तीन किनारों वाला पथ जिसमें कोने साफ दिखते हैं.',
      'Figure 8': 'थोड़ा ज्यादा playful एहसास वाला 8-आकार लूप.',
      Parallelogram: 'तिरछी ऊर्जा वाला ज्यादा साफ ज्यामितीय पथ.',
      Random: 'आजाद बिंदु जो फिर भी सीमा में रहते हैं.',
    },
    ctaEyebrow: 'डेस्कटॉप वितरण',
    ctaTitle: 'अगला कदम Windows build को GitHub Releases पर प्रकाशित करना है.',
    ctaBody:
      'ऐप को वितरित करने का सबसे साफ तरीका GitHub Releases है. इससे आपको एक असली डाउनलोड पेज मिलता है और बड़ा executable repo history को भारी नहीं बनाता.',
    openReleases: 'Releases खोलें',
    faqEyebrow: 'सवाल',
    faqTitle: 'आम सवाल',
    faqs: [
      {
        question: 'क्या यह वेबसाइट मेरे असली माउस को चला सकती है?',
        answer:
          'नहीं. ब्राउज़र सिस्टम माउस को नियंत्रित नहीं कर सकते. यह वेबसाइट ब्रांड, प्रोडक्ट स्टोरी और विज़ुअल डेमो के लिए है. असली utility डेस्कटॉप ऐप में है.',
      },
      {
        question: 'WIGGLER किन पैटर्न्स को सपोर्ट करता है?',
        answer:
          'Circle, Square, Triangle, Figure 8, Parallelogram और Random सभी डेस्कटॉप ऐप में सपोर्टेड हैं और वेबसाइट डेमो में भी दिखते हैं.',
      },
      {
        question: 'ऐप को प्रकाशित करने का सबसे अच्छा तरीका क्या है?',
        answer:
          'Windows version के लिए GitHub Releases सबसे साफ जगह है. इससे executable सामान्य repo history से बाहर रहता है और एक सही download page मिलता है.',
      },
    ],
  },
}

const languageOptions = Object.fromEntries(
  Object.entries(translations).map(([code, value]) => [code, value.languageName]),
)

const storageKeys = {
  theme: 'wiggler-theme',
  language: 'wiggler-language',
}

function getInitialLanguage() {
  const stored = window.localStorage.getItem(storageKeys.language)
  if (stored && translations[stored]) {
    return stored
  }

  const browserLanguage = window.navigator.language.toLowerCase()
  const match = Object.keys(translations).find((code) => browserLanguage.startsWith(code))
  return match ?? 'en'
}

function getCopy(language) {
  const text = translations[language] ?? translations.en
  return {
    ...text,
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
              {text.patternNames[pattern]}
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
        <img src={settingsIconUrl} alt="" />
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
                  <strong>{text.patternNames.Circle}</strong>
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
              <span key={pattern}>{text.patternNames[pattern]}</span>
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
