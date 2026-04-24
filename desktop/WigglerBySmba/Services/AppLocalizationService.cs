using WigglerBySmba.Models;

namespace WigglerBySmba.Services;

public sealed class AppLocalizationService
{
    private static readonly Dictionary<string, AppTextPack?> Packs = new()
    {
        ["en"] = new(
            "English", "Ready", "Off", "Running", "Theme", "Language", "Mode", "Color vibe", "Launch in",
            "Close behavior", "Idle delay", "Pattern", "Speed", "Size", "Settings", "Shape the vibe, language, tray behavior, and movement feel from one place.",
            "Quiet control for your cursor", "Now tuned for", "Smooth movement, instant interruption, and tighter tray behavior.",
            "Behavior", "Takeover", "Your mouse wins instantly", "How it feels", "WIGGLER waits for you to go idle, then traces visible shapes across the screen. The moment you touch the mouse, it stops and hands control back.",
            "Replay tutorial", "Hide settings", "Open settings", "Tray-ready", "Set launch mode to Tray if you want WIGGLER tucked away. Close behavior can either exit completely or minimize to the tray and keep running.",
            "Look", "Behavior", "Movement", "Status", "Settings", "WIGGLER", "by SMBA", "Idle delay", "Pattern", "Ready", "Turn ON", "Turn OFF",
            "Off until you say otherwise.", "Ready and waiting for idle.", "Gliding until you take back control.",
            "WIGGLER is off.", "Ready. It starts after {0} of inactivity.", "Active. Movement stops the instant you touch the mouse.",
            "Idle monitoring is paused.", "Watching for {0} before starting movement.", "Running a smooth {0} path now. Real mouse input cuts in immediately.",
            new[]
            {
                new TutorialPageText("A calm mouse wiggler with boundaries.", "Turn WIGGLER on, step away, and it waits until you have actually been idle before it takes over.", "It stays friendly by waiting first, then moving only after the idle timer expires."),
                new TutorialPageText("Turning it on is the whole flow.", "Use the main power button to turn it on. While ready, WIGGLER watches for real user input and stays out of your way.", "The default idle delay is 12 seconds, and you can change that any time in Settings."),
                new TutorialPageText("Patterns stay visible and controlled.", "Choose Circle, Square, Triangle, Figure 8, Parallelogram, or Random.", "Patterns stay on-screen, redirect near edges, and travel smoothly over time instead of jumping around."),
                new TutorialPageText("You always win instantly.", "The moment you move the mouse yourself, WIGGLER stops immediately and returns to the ready state.", "Its own movement is tagged and ignored, so it never mistakes itself for your input."),
                new TutorialPageText("Tray support keeps it tucked away.", "Run it in the background, reopen it from the tray, and replay this tutorial any time from Settings.", "You can choose whether the app launches in a window or the tray, and whether closing hides it or exits.")
            }),
        ["es"] = new(
            "Español", "Listo", "Apagado", "Activo", "Tema", "Idioma", "Modo", "Color", "Abrir en",
            "Al cerrar", "Espera inactiva", "Patrón", "Velocidad", "Tamaño", "Ajustes", "Ajusta el estilo, el idioma, el comportamiento en bandeja y la sensación del movimiento desde un solo lugar.",
            "Control tranquilo para tu cursor", "Ahora afinado para", "Movimiento suave, interrupción instantánea y mejor comportamiento en bandeja.",
            "Comportamiento", "Control", "Tu mouse gana al instante", "Cómo se siente", "WIGGLER espera a que quedes inactivo y luego dibuja formas visibles en pantalla. En cuanto tocas el mouse, se detiene y te devuelve el control.",
            "Repetir tutorial", "Ocultar ajustes", "Abrir ajustes", "Listo para bandeja", "Elige Bandeja si quieres que WIGGLER quede guardado. Al cerrar puede salir por completo o minimizarse a la bandeja y seguir funcionando.",
            "Apariencia", "Comportamiento", "Movimiento", "Estado", "Ajustes", "WIGGLER", "by SMBA", "Espera", "Patrón", "Listo", "Encender", "Apagar",
            "Apagado hasta que tú lo decidas.", "Listo y esperando inactividad.", "Deslizándose hasta que recuperes el control.",
            "WIGGLER está apagado.", "Listo. Empieza después de {0} de inactividad.", "Activo. El movimiento se detiene en cuanto tocas el mouse.",
            "La supervisión de inactividad está pausada.", "Esperando {0} antes de empezar.", "Ahora ejecuta un recorrido {0} suave. La entrada real del mouse corta de inmediato.",
            null),
        ["pt"] = new(
            "Português", "Pronto", "Desligado", "Ativo", "Tema", "Idioma", "Modo", "Cor", "Abrir em",
            "Ao fechar", "Espera ociosa", "Padrão", "Velocidade", "Tamanho", "Ajustes", "Ajuste o visual, o idioma, o comportamento na bandeja e a sensação do movimento em um só lugar.",
            "Controle calmo para o seu cursor", "Agora ajustado para", "Movimento suave, interrupção imediata e bandeja mais consistente.",
            "Comportamento", "Controle", "Seu mouse vence na hora", "Como funciona", "WIGGLER espera você ficar ocioso e então desenha formas visíveis pela tela. No momento em que você toca o mouse, ele para e devolve o controle.",
            "Rever tutorial", "Ocultar ajustes", "Abrir ajustes", "Pronto para bandeja", "Escolha Bandeja se quiser deixar o WIGGLER guardado. Ao fechar, ele pode sair de vez ou ir para a bandeja e continuar rodando.",
            "Visual", "Comportamento", "Movimento", "Status", "Ajustes", "WIGGLER", "by SMBA", "Espera", "Padrão", "Pronto", "Ligar", "Desligar",
            "Desligado até você decidir.", "Pronto e esperando a inatividade.", "Deslizando até você retomar o controle.",
            "WIGGLER está desligado.", "Pronto. Ele começa depois de {0} de inatividade.", "Ativo. O movimento para no instante em que você toca o mouse.",
            "O monitoramento de inatividade está pausado.", "Esperando {0} antes de começar.", "Executando agora um trajeto {0} suave. A entrada real do mouse corta imediatamente.",
            null),
        ["fr"] = new(
            "Français", "Prêt", "Éteint", "Actif", "Thème", "Langue", "Mode", "Couleur", "Lancer en",
            "Au fermeture", "Délai d’inactivité", "Motif", "Vitesse", "Taille", "Paramètres", "Réglez le style, la langue, le comportement du tray et la sensation du mouvement depuis un seul endroit.",
            "Un contrôle calme pour votre curseur", "Maintenant réglé pour", "Mouvement fluide, interruption instantanée et tray plus propre.",
            "Comportement", "Prise en main", "Votre souris gagne instantanément", "Le ressenti", "WIGGLER attend que vous deveniez inactif puis trace des formes visibles à l’écran. Dès que vous touchez la souris, il s’arrête et vous rend la main.",
            "Rejouer le tutoriel", "Masquer les paramètres", "Ouvrir les paramètres", "Prêt pour le tray", "Choisissez Tray si vous voulez garder WIGGLER discret. À la fermeture, il peut quitter complètement ou se réduire dans le tray et continuer.",
            "Apparence", "Comportement", "Mouvement", "Statut", "Paramètres", "WIGGLER", "by SMBA", "Délai", "Motif", "Prêt", "Activer", "Désactiver",
            "Éteint jusqu’à votre décision.", "Prêt et en attente d’inactivité.", "En glisse jusqu’à ce que vous repreniez la main.",
            "WIGGLER est éteint.", "Prêt. Il démarre après {0} d’inactivité.", "Actif. Le mouvement s’arrête dès que vous touchez la souris.",
            "La surveillance d’inactivité est en pause.", "En attente de {0} avant de démarrer.", "Exécute maintenant un trajet {0} fluide. L’entrée réelle de la souris coupe immédiatement.",
            null),
        ["de"] = new(
            "Deutsch", "Bereit", "Aus", "Aktiv", "Design", "Sprache", "Modus", "Farbwelt", "Starten in",
            "Beim Schließen", "Leerlaufzeit", "Muster", "Geschwindigkeit", "Größe", "Einstellungen", "Passe Stil, Sprache, Tray-Verhalten und Bewegungsgefühl an einem Ort an.",
            "Ruhige Kontrolle für deinen Cursor", "Jetzt abgestimmt auf", "Sanfte Bewegung, sofortige Unterbrechung und saubereres Tray-Verhalten.",
            "Verhalten", "Übernahme", "Deine Maus gewinnt sofort", "So fühlt es sich an", "WIGGLER wartet auf Leerlauf und zeichnet dann sichtbare Formen über den Bildschirm. Sobald du die Maus berührst, stoppt es und gibt dir die Kontrolle zurück.",
            "Tutorial erneut abspielen", "Einstellungen ausblenden", "Einstellungen öffnen", "Tray-bereit", "Wähle Tray, wenn WIGGLER verborgen bleiben soll. Beim Schließen kann es komplett beenden oder ins Tray minimieren und weiterlaufen.",
            "Aussehen", "Verhalten", "Bewegung", "Status", "Einstellungen", "WIGGLER", "by SMBA", "Leerlauf", "Muster", "Bereit", "Einschalten", "Ausschalten",
            "Aus, bis du es einschaltest.", "Bereit und wartet auf Leerlauf.", "Gleitet, bis du die Kontrolle zurücknimmst.",
            "WIGGLER ist aus.", "Bereit. Es startet nach {0} Inaktivität.", "Aktiv. Die Bewegung stoppt sofort, wenn du die Maus berührst.",
            "Die Leerlaufüberwachung ist pausiert.", "Wartet {0}, bevor die Bewegung startet.", "Führt jetzt einen sanften {0}-Pfad aus. Echte Mauseingaben unterbrechen sofort.",
            null),
        ["it"] = new(
            "Italiano", "Pronto", "Spento", "Attivo", "Tema", "Lingua", "Modalità", "Colore", "Avvia in",
            "Alla chiusura", "Attesa inattiva", "Schema", "Velocità", "Dimensione", "Impostazioni", "Regola stile, lingua, comportamento nel tray e sensazione del movimento da un solo posto.",
            "Controllo calmo per il cursore", "Ora ottimizzato per", "Movimento fluido, interruzione istantanea e tray più solido.",
            "Comportamento", "Ripresa", "Il tuo mouse vince subito", "Come si sente", "WIGGLER aspetta che tu diventi inattivo e poi traccia forme visibili sullo schermo. Appena tocchi il mouse, si ferma e restituisce il controllo.",
            "Ripeti tutorial", "Nascondi impostazioni", "Apri impostazioni", "Pronto per il tray", "Scegli Tray se vuoi tenere WIGGLER nascosto. Alla chiusura può uscire del tutto oppure ridursi nel tray e continuare.",
            "Aspetto", "Comportamento", "Movimento", "Stato", "Impostazioni", "WIGGLER", "by SMBA", "Attesa", "Schema", "Pronto", "Accendi", "Spegni",
            "Spento finché non lo decidi tu.", "Pronto e in attesa di inattività.", "Scorre finché non riprendi il controllo.",
            "WIGGLER è spento.", "Pronto. Parte dopo {0} di inattività.", "Attivo. Il movimento si ferma nell’istante in cui tocchi il mouse.",
            "Il monitoraggio dell’inattività è in pausa.", "In attesa di {0} prima di iniziare.", "Sta eseguendo ora un percorso {0} fluido. L’input reale del mouse interrompe subito.",
            null),
        ["nl"] = new(
            "Nederlands", "Klaar", "Uit", "Actief", "Thema", "Taal", "Modus", "Kleur", "Open in",
            "Bij sluiten", "Wachttijd", "Patroon", "Snelheid", "Grootte", "Instellingen", "Pas stijl, taal, traygedrag en bewegingsgevoel aan vanuit één plek.",
            "Rustige controle voor je cursor", "Nu afgestemd op", "Vloeiende beweging, directe onderbreking en strakker traygedrag.",
            "Gedrag", "Overname", "Jouw muis wint meteen", "Hoe het voelt", "WIGGLER wacht tot je inactief bent en tekent dan zichtbare vormen over het scherm. Zodra je de muis aanraakt, stopt het en krijg je direct weer controle.",
            "Tutorial opnieuw", "Instellingen verbergen", "Instellingen openen", "Klaar voor tray", "Kies Tray als je WIGGLER uit beeld wilt houden. Bij sluiten kan het volledig afsluiten of naar de tray gaan en blijven draaien.",
            "Uiterlijk", "Gedrag", "Beweging", "Status", "Instellingen", "WIGGLER", "by SMBA", "Wachttijd", "Patroon", "Klaar", "Aan", "Uit",
            "Uit totdat jij iets zegt.", "Klaar en wacht op inactiviteit.", "Beweegt soepel totdat jij het overneemt.",
            "WIGGLER staat uit.", "Klaar. Het start na {0} inactiviteit.", "Actief. De beweging stopt zodra je de muis aanraakt.",
            "Inactiviteitscontrole is gepauzeerd.", "Wacht {0} voordat beweging start.", "Voert nu een soepel {0}-pad uit. Echte muisinvoer onderbreekt meteen.",
            null),
        ["sv"] = new(
            "Svenska", "Redo", "Av", "Aktiv", "Tema", "Språk", "Läge", "Färg", "Starta i",
            "Vid stängning", "Vilofördröjning", "Mönster", "Hastighet", "Storlek", "Inställningar", "Justera stil, språk, traybeteende och rörelsekänsla på ett ställe.",
            "Lugn kontroll för din markör", "Nu anpassad för", "Mjuk rörelse, omedelbar avbrytning och bättre traybeteende.",
            "Beteende", "Övertag", "Din mus vinner direkt", "Så känns det", "WIGGLER väntar tills du blir inaktiv och ritar sedan synliga former över skärmen. Så fort du rör musen stannar det och lämnar tillbaka kontrollen.",
            "Spela upp tutorial igen", "Dölj inställningar", "Öppna inställningar", "Redo för tray", "Välj Tray om du vill hålla WIGGLER undanstoppad. Vid stängning kan den avslutas helt eller minimera till tray och fortsätta köra.",
            "Utseende", "Beteende", "Rörelse", "Status", "Inställningar", "WIGGLER", "by SMBA", "Vila", "Mönster", "Redo", "Slå på", "Slå av",
            "Av tills du säger till.", "Redo och väntar på inaktivitet.", "Glider tills du tar tillbaka kontrollen.",
            "WIGGLER är av.", "Redo. Den startar efter {0} inaktivitet.", "Aktiv. Rörelsen stoppar direkt när du rör musen.",
            "Övervakning av inaktivitet är pausad.", "Väntar {0} innan rörelse startar.", "Kör nu en mjuk {0}-bana. Riktig musinmatning bryter direkt.",
            null),
        ["ja"] = null!,
        ["ko"] = null!,
        ["zh"] = null!,
        ["ar"] = null!,
        ["hi"] = null!
    };

    public AppLocalizationService()
    {
        Packs["ja"] = new(
            "日本語", "待機中", "オフ", "動作中", "テーマ", "言語", "モード", "カラーバイブ", "起動方法", "閉じる動作", "待機時間", "パターン", "速度", "サイズ", "設定",
            "見た目、言語、トレイ動作、動きの感触をここでまとめて調整できます。",
            "カーソルのための静かなコントロール", "現在の設定", "なめらかな動き、即時停止、より自然なトレイ動作。",
            "動作", "優先権", "マウスがすぐに勝ちます", "使い心地", "WIGGLER はユーザーが操作をやめるまで待ってから、画面上に見える形で動きます。マウスに触れた瞬間に止まり、操作を返します。",
            "チュートリアルを再表示", "設定を隠す", "設定を開く", "トレイ対応", "トレイ起動を選ぶと WIGGLER を邪魔にならない場所に置けます。閉じる動作は完全終了か、トレイに最小化して継続のどちらかです。",
            "見た目", "動作", "移動", "状態", "設定", "WIGGLER", "by SMBA", "待機", "パターン", "待機中", "オン", "オフ",
            "必要になるまでオフです。", "待機中で、無操作を待っています。", "操作を返すまで滑らかに動いています。",
            "WIGGLER はオフです。", "待機中。{0} の無操作後に始まります。", "動作中。マウスに触れた瞬間に停止します。",
            "無操作の監視は停止中です。", "{0} を待ってから開始します。", "いま {0} パスを滑らかに実行中です。実際のマウス入力がすぐに優先されます。",
            null);
        Packs["ko"] = new(
            "한국어", "대기 중", "꺼짐", "실행 중", "테마", "언어", "모드", "컬러 무드", "실행 위치", "닫기 동작", "대기 시간", "패턴", "속도", "크기", "설정",
            "스타일, 언어, 트레이 동작, 움직임 느낌을 한 곳에서 조절할 수 있습니다.",
            "커서를 위한 조용한 제어", "현재 설정", "부드러운 움직임, 즉시 중단, 더 자연스러운 트레이 동작.",
            "동작", "우선권", "마우스가 즉시 이깁니다", "사용 느낌", "WIGGLER 는 사용자가 멈출 때까지 기다렸다가 화면 위에 보이는 형태로 움직입니다. 마우스를 건드리는 순간 바로 멈추고 제어권을 돌려줍니다.",
            "튜토리얼 다시 보기", "설정 숨기기", "설정 열기", "트레이 준비", "트레이 실행을 선택하면 WIGGLER 를 조용히 숨길 수 있습니다. 닫기 동작은 완전 종료 또는 트레이 최소화 후 계속 실행 중 하나를 선택할 수 있습니다.",
            "모양", "동작", "이동", "상태", "설정", "WIGGLER", "by SMBA", "대기", "패턴", "대기 중", "켜기", "끄기",
            "필요할 때까지 꺼져 있습니다.", "대기 중이며 유휴 상태를 기다립니다.", "제어권을 돌려줄 때까지 부드럽게 움직입니다.",
            "WIGGLER 는 꺼져 있습니다.", "대기 중. {0} 동안 입력이 없으면 시작합니다.", "실행 중. 마우스를 건드리는 순간 움직임이 멈춥니다.",
            "유휴 감시는 일시 중지되었습니다.", "{0} 후에 시작됩니다.", "지금 {0} 경로를 부드럽게 실행 중입니다. 실제 마우스 입력이 즉시 우선됩니다.",
            null);
        Packs["zh"] = new(
            "中文", "待命", "关闭", "运行中", "主题", "语言", "模式", "颜色氛围", "启动位置", "关闭行为", "空闲延迟", "图形", "速度", "大小", "设置",
            "在这里统一调整外观、语言、托盘行为和移动手感。",
            "为你的光标准备的安静控制", "当前已优化为", "更顺滑的移动、即时打断和更自然的托盘体验。",
            "行为", "控制权", "你的鼠标会立刻获胜", "使用感受", "WIGGLER 会先等待用户空闲，然后在屏幕上画出可见的移动形状。你一碰鼠标，它就会停止并立刻交还控制权。",
            "重播教程", "隐藏设置", "打开设置", "支持托盘", "如果你希望 WIGGLER 藏在后台，可以选择托盘启动。关闭时可以选择完全退出，或最小化到托盘继续运行。",
            "外观", "行为", "移动", "状态", "设置", "WIGGLER", "by SMBA", "空闲", "图形", "待命", "开启", "关闭",
            "在你需要之前保持关闭。", "待命中，正在等待空闲。", "正在顺滑移动，直到你重新接管。",
            "WIGGLER 当前已关闭。", "待命中。空闲 {0} 后开始。", "运行中。你一碰鼠标就会立刻停止。",
            "空闲监控已暂停。", "将在 {0} 后开始。", "当前正在执行顺滑的 {0} 路径。真实鼠标输入会立即打断。",
            null);
        Packs["ar"] = new(
            "العربية", "جاهز", "متوقف", "قيد التشغيل", "السمة", "اللغة", "الوضع", "لون الواجهة", "فتح في", "سلوك الإغلاق", "مهلة الخمول", "النمط", "السرعة", "الحجم", "الإعدادات",
            "اضبط المظهر واللغة وسلوك الدرج وإحساس الحركة من مكان واحد.",
            "تحكم هادئ للمؤشر", "مضبوط الآن على", "حركة أنعم، توقف فوري، وسلوك درج أفضل.",
            "السلوك", "التحكم", "الفأرة تربح فوراً", "الإحساس", "ينتظر WIGGLER حتى تصبح غير نشط ثم يرسم مسارات واضحة على الشاشة. وبمجرد لمس الفأرة يتوقف ويعيد التحكم فوراً.",
            "إعادة الشرح", "إخفاء الإعدادات", "فتح الإعدادات", "جاهز للدرج", "اختر التشغيل في الدرج إذا أردت إبقاء WIGGLER بعيداً عن الواجهة. عند الإغلاق يمكنه الخروج تماماً أو التصغير إلى الدرج والاستمرار.",
            "المظهر", "السلوك", "الحركة", "الحالة", "الإعدادات", "WIGGLER", "by SMBA", "الخمول", "النمط", "جاهز", "تشغيل", "إيقاف",
            "متوقف حتى تقرر تشغيله.", "جاهز وينتظر الخمول.", "ينساب حتى تستعيد التحكم.",
            "WIGGLER متوقف.", "جاهز. يبدأ بعد {0} من الخمول.", "قيد التشغيل. تتوقف الحركة فور لمس الفأرة.",
            "مراقبة الخمول متوقفة مؤقتاً.", "ينتظر {0} قبل البدء.", "يشغّل الآن مسار {0} بسلاسة. إدخال الفأرة الحقيقي يقطع فوراً.",
            null);
        Packs["hi"] = new(
            "हिन्दी", "तैयार", "बंद", "चल रहा है", "थीम", "भाषा", "मोड", "रंग शैली", "कहाँ खुले", "बंद करने पर", "निष्क्रिय देरी", "पैटर्न", "स्पीड", "साइज़", "सेटिंग्स",
            "एक ही जगह से लुक, भाषा, ट्रे व्यवहार और मूवमेंट की फील को बदलें.",
            "आपके कर्सर के लिए शांत नियंत्रण", "अभी सेट है", "और स्मूद मूवमेंट, तुरंत रुकना, और बेहतर ट्रे व्यवहार.",
            "व्यवहार", "कंट्रोल", "आपका माउस तुरंत जीतता है", "कैसा महसूस होता है", "WIGGLER तब तक इंतजार करता है जब तक आप निष्क्रिय नहीं हो जाते, फिर स्क्रीन पर साफ़ आकारों में चलता है. जैसे ही आप माउस छूते हैं, यह रुक जाता है और कंट्रोल वापस दे देता है.",
            "ट्यूटोरियल फिर चलाएँ", "सेटिंग्स छुपाएँ", "सेटिंग्स खोलें", "ट्रे के लिए तैयार", "अगर आप WIGGLER को छुपाकर रखना चाहते हैं तो Tray चुनें. बंद करने पर यह पूरी तरह बंद हो सकता है या ट्रे में जाकर चलता रह सकता है.",
            "लुक", "व्यवहार", "मूवमेंट", "स्थिति", "सेटिंग्स", "WIGGLER", "by SMBA", "निष्क्रिय", "पैटर्न", "तैयार", "चालू", "बंद",
            "जब तक आप न चाहें, यह बंद रहता है.", "तैयार है और निष्क्रियता का इंतजार कर रहा है.", "आपके कंट्रोल लेने तक स्मूद चल रहा है.",
            "WIGGLER बंद है.", "तैयार. यह {0} की निष्क्रियता के बाद शुरू होता है.", "चल रहा है. जैसे ही आप माउस छूते हैं, मूवमेंट रुक जाता है.",
            "निष्क्रियता निगरानी रुकी हुई है.", "{0} के बाद शुरू होगा.", "अभी स्मूद {0} पथ चला रहा है. असली माउस इनपुट तुरंत बीच में आ जाता है.",
            null);
    }

    public AppTextPack GetPack(string languageCode)
    {
        if (Packs.TryGetValue(languageCode, out var pack) && pack is not null)
        {
            return pack with
            {
                TutorialPages = pack.TutorialPages ?? Packs["en"]!.TutorialPages
            };
        }

        return Packs["en"]!;
    }

    public IReadOnlyList<UiOption<string>> GetLanguageOptions() =>
        Packs.Select(pair => new UiOption<string>(pair.Key, GetPack(pair.Key).LanguageName)).ToList();

    public sealed record AppTextPack(
        string LanguageName,
        string ReadyStatus,
        string OffStatus,
        string RunningStatus,
        string ThemeLabel,
        string LanguageLabel,
        string ModeLabel,
        string ColorVibeLabel,
        string LaunchInLabel,
        string CloseBehaviorLabel,
        string IdleDelayLabel,
        string PatternLabel,
        string SpeedLabel,
        string SizeLabel,
        string SettingsTitle,
        string SettingsSubtitle,
        string HeroEyebrow,
        string NowTunedForLabel,
        string HeroCardDetail,
        string BehaviorTitle,
        string TakeoverTitle,
        string TakeoverValue,
        string HowItFeelsTitle,
        string HowItFeelsBody,
        string ReplayTutorialLabel,
        string HideSettingsLabel,
        string OpenSettingsLabel,
        string TrayReadyTitle,
        string TrayReadyBody,
        string LookSectionTitle,
        string BehaviorSectionTitle,
        string MovementSectionTitle,
        string StatusTitle,
        string SettingsHeaderTitle,
        string BrandTitle,
        string BrandSubtitle,
        string IdleDelayCardTitle,
        string PatternCardTitle,
        string ReadyCardValue,
        string ToggleOnLabel,
        string ToggleOffLabel,
        string HeroOffLabel,
        string HeroReadyLabel,
        string HeroRunningLabel,
        string BehaviorOffSummary,
        string BehaviorReadySummary,
        string BehaviorRunningSummary,
        string StatusOffDetail,
        string StatusReadyDetail,
        string StatusRunningDetail,
        TutorialPageText[]? TutorialPages);

    public sealed record TutorialPageText(string Title, string Body, string Detail);
}
