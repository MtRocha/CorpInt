/**
 * Sistema de Animações para Intranet
 * Inclui animações de aniversário e conquistas estilo Wild Rift
 */

// Classe principal para gerenciar animações
class AnimationSystem {
    constructor() {
        this.initialized = false
        this.achievements = []
        this.achievementQueue = []
        this.currentAchievement = null
        this.processingQueue = false
        this.fireworksActive = false
    }

    // Inicializa o sistema de animações
    init() {
        if (this.initialized) return

        // Criar containers para animações
        this.createContainers()

        // Verificar se é aniversário
        this.checkBirthday()

        this.initialized = true

        // Processar fila de conquistas
        setInterval(() => this.processAchievementQueue(), 500)
    }

    // Cria os containers necessários para as animações
    createContainers() {
        // Container principal
        this.container = document.createElement("div")
        this.container.className = "animation-container"
        document.body.appendChild(this.container)

        // Container para animação de aniversário
        this.birthdayContainer = document.createElement("div")
        this.birthdayContainer.className = "birthday-animation"
        this.container.appendChild(this.birthdayContainer)

        // Canvas para fogos de artifício
        this.canvas = document.createElement("canvas")
        this.canvas.id = "fireworks-canvas"
        this.container.appendChild(this.canvas)
    }

    // Verifica se é aniversário do usuário
    checkBirthday() {
        // Verificar a variável ViewBag.Aniversario
        const isUserBirthday = document.getElementById("is-birthday")

        if (isUserBirthday && isUserBirthday.value === "true") {
            this.showBirthdayAnimation()
        }
    }

    // Exibe a animação de aniversário
    showBirthdayAnimation() {
        // Limpar o container
        this.birthdayContainer.innerHTML = ""

        // Criar elementos da animação
        const message = document.createElement("div")
        message.className = "birthday-message"
        message.textContent = "Feliz Aniversário!"

        const subMessage = document.createElement("div")
        subMessage.className = "birthday-submessage"
        subMessage.textContent = "Desejamos um dia incrível!"

        // Adicionar ao container
        this.birthdayContainer.appendChild(message)
        this.birthdayContainer.appendChild(subMessage)

        // Ativar a animação
        setTimeout(() => {
            this.birthdayContainer.classList.add("active")
            this.startFireworks()
            this.createConfetti()

            // Reproduzir som (opcional)
            this.playSound("birthday")

            // Esconder após alguns segundos
            setTimeout(() => {
                this.birthdayContainer.classList.remove("active")
                setTimeout(() => {
                    this.stopFireworks()
                }, 500)
            }, 5000)
        }, 1000)
    }

    // Inicia a animação de fogos de artifício
    startFireworks() {
        if (this.fireworksActive) return

        this.fireworksActive = true
        this.initFireworks()
    }

    // Para a animação de fogos de artifício
    stopFireworks() {
        this.fireworksActive = false

        // Limpar o canvas
        const ctx = this.canvas.getContext("2d")
        ctx.clearRect(0, 0, this.canvas.width, this.canvas.height)
    }

    // Inicializa os fogos de artifício
    initFireworks() {
        const canvas = this.canvas
        const ctx = canvas.getContext("2d")

        // Configurar canvas
        canvas.width = window.innerWidth
        canvas.height = window.innerHeight

        const fireworks = []
        const particles = []

        // Função para criar fogos de artifício
        const createFirework = () => {
            const x = Math.random() * canvas.width
            const y = canvas.height
            const targetX = Math.random() * canvas.width
            const targetY = (Math.random() * canvas.height) / 2

            fireworks.push({
                x,
                y,
                targetX,
                targetY,
                color: `hsl(${Math.random() * 360}, 100%, 50%)`,
                size: 2,
                speed: 2 + Math.random() * 3,
            })
        }

        // Função para criar partículas
        const createParticles = (x, y, color) => {
            const particleCount = 30 + Math.floor(Math.random() * 30)

            for (let i = 0; i < particleCount; i++) {
                const angle = Math.random() * Math.PI * 2
                const speed = 1 + Math.random() * 3

                particles.push({
                    x,
                    y,
                    vx: Math.cos(angle) * speed,
                    vy: Math.sin(angle) * speed,
                    color,
                    size: 1 + Math.random() * 2,
                    alpha: 1,
                    gravity: 0.05,
                })
            }
        }

        // Função de animação
        const animate = () => {
            if (!this.fireworksActive) return

            // Limpar canvas
            ctx.fillStyle = "rgba(0, 0, 0, 0.1)"
            ctx.fillRect(0, 0, canvas.width, canvas.height)

            // Atualizar e desenhar fogos de artifício
            for (let i = fireworks.length - 1; i >= 0; i--) {
                const firework = fireworks[i]

                // Calcular direção
                const dx = firework.targetX - firework.x
                const dy = firework.targetY - firework.y
                const distance = Math.sqrt(dx * dx + dy * dy)

                if (distance < 5 || Math.random() < 0.03) {
                    // Explodir
                    createParticles(firework.x, firework.y, firework.color)
                    fireworks.splice(i, 1)
                } else {
                    // Mover em direção ao alvo
                    firework.x += (dx / distance) * firework.speed
                    firework.y += (dy / distance) * firework.speed

                    // Desenhar
                    ctx.fillStyle = firework.color
                    ctx.beginPath()
                    ctx.arc(firework.x, firework.y, firework.size, 0, Math.PI * 2)
                    ctx.fill()
                }
            }

            // Atualizar e desenhar partículas
            for (let i = particles.length - 1; i >= 0; i--) {
                const particle = particles[i]

                // Atualizar posição
                particle.x += particle.vx
                particle.y += particle.vy
                particle.vy += particle.gravity
                particle.alpha -= 0.01

                // Remover partículas invisíveis
                if (particle.alpha <= 0) {
                    particles.splice(i, 1)
                    continue
                }

                // Desenhar
                ctx.fillStyle = particle.color.replace(")", `, ${particle.alpha})`).replace("rgb", "rgba")
                ctx.beginPath()
                ctx.arc(particle.x, particle.y, particle.size, 0, Math.PI * 2)
                ctx.fill()
            }

            // Criar novos fogos de artifício
            if (Math.random() < 0.05 && fireworks.length < 5) {
                createFirework()
            }

            requestAnimationFrame(animate)
        }

        // Iniciar animação
        createFirework()
        animate()
    }

    // Cria confetes para a animação de aniversário
    createConfetti() {
        const colors = ["#f94144", "#f3722c", "#f8961e", "#f9c74f", "#90be6d", "#43aa8b", "#577590"]
        const confettiCount = 200

        for (let i = 0; i < confettiCount; i++) {
            setTimeout(() => {
                const confetti = document.createElement("div")
                confetti.className = "confetti"
                confetti.style.left = `${Math.random() * 100}%`
                confetti.style.backgroundColor = colors[Math.floor(Math.random() * colors.length)]
                confetti.style.width = `${5 + Math.random() * 10}px`
                confetti.style.height = `${5 + Math.random() * 10}px`
                confetti.style.animationDuration = `${3 + Math.random() * 4}s`
                confetti.style.animationDelay = `${Math.random() * 2}s`

                this.container.appendChild(confetti)

                // Remover após a animação
                setTimeout(() => {
                    confetti.remove()
                }, 7000)
            }, Math.random() * 1000)
        }
    }

    // Exibe uma conquista estilo Wild Rift
    showAchievement(options) {
        const defaultOptions = {
            title: "CONQUISTA DESBLOQUEADA",
            message: "Nova conquista",
            description: "Você desbloqueou uma nova conquista!",
            icon: "trophy",
            progress: 100,
            duration: 5000,
            sound: "achievement",
        }

        const config = { ...defaultOptions, ...options }

        // Adicionar à fila
        this.achievementQueue.push(config)

        // Processar fila se não estiver processando
        if (!this.processingQueue) {
            this.processAchievementQueue()
        }
    }

    // Processa a fila de conquistas
    processAchievementQueue() {
        if (this.processingQueue || this.achievementQueue.length === 0) return

        this.processingQueue = true
        const achievement = this.achievementQueue.shift()
        this.displayAchievement(achievement)
    }

    // Exibe uma conquista na tela
    displayAchievement(achievement) {
        // Criar container da conquista
        const container = document.createElement("div")
        container.className = "achievement-container"

        // Criar ícone
        const icon = document.createElement("div")
        icon.className = "achievement-icon"
        icon.innerHTML = `<i class="fas fa-${achievement.icon}"></i>`

        // Criar conteúdo
        const content = document.createElement("div")
        content.className = "achievement-content"

        // Título
        const title = document.createElement("div")
        title.className = "achievement-title"
        title.textContent = achievement.title

        // Mensagem
        const message = document.createElement("div")
        message.className = "achievement-message"
        message.textContent = achievement.message

        // Descrição
        const description = document.createElement("div")
        description.className = "achievement-description"
        description.textContent = achievement.description

        // Barra de progresso
        const progress = document.createElement("div")
        progress.className = "achievement-progress"

        const progressBar = document.createElement("div")
        progressBar.className = "achievement-progress-bar"
        progress.appendChild(progressBar)

        // Botão de fechar
        const closeBtn = document.createElement("button")
        closeBtn.className = "achievement-close"
        closeBtn.innerHTML = '<i class="fas fa-times"></i>'
        closeBtn.addEventListener("click", () => {
            this.hideAchievement(container)
        })

        // Montar componentes
        content.appendChild(title)
        content.appendChild(message)
        content.appendChild(description)
        content.appendChild(progress)

        container.appendChild(icon)
        container.appendChild(content)
        container.appendChild(closeBtn)

        // Adicionar ao DOM
        document.body.appendChild(container)

        // Animar entrada
        setTimeout(() => {
            container.classList.add("active")

            // Animar barra de progresso
            setTimeout(() => {
                progressBar.style.width = `${achievement.progress}%`
            }, 300)

            // Reproduzir som
            this.playSound(achievement.sound)

            // Esconder após duração
            setTimeout(() => {
                this.hideAchievement(container)
            }, achievement.duration)
        }, 100)

        // Salvar referência
        this.currentAchievement = container
    }

    // Esconde uma conquista
    hideAchievement(container) {
        container.classList.remove("active")

        // Remover após animação
        setTimeout(() => {
            container.remove()
            this.currentAchievement = null
            this.processingQueue = false

            // Processar próxima conquista
            this.processAchievementQueue()
        }, 500)
    }

    // Reproduz um som
    playSound(type) {
        // Implementar reprodução de som se necessário
        // Exemplo:
        // const sound = new Audio(`/sounds/${type}.mp3`);
        // sound.play();
    }
}

// Inicializar o sistema de animações
document.addEventListener("DOMContentLoaded", () => {
    window.AnimationSystem = new AnimationSystem()
    window.AnimationSystem.init()

    // Expor função para mostrar conquistas
    window.showAchievement = (options) => {
        window.AnimationSystem.showAchievement(options)
    }
})
