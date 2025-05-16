/**
 * Sistema de Animação de Aniversário Expandido
 * Suporta aniversários pessoais, da empresa e tempo de casa
 */

class BirthdayAnimation {
    constructor() {
        this.initialized = false
        this.active = false
    }

    // Inicializa o sistema de animação
    init() {
        if (this.initialized) return

        // Verificar se é aniversário pessoal, da empresa ou tempo de casa
        this.checkBirthday()

        this.initialized = true
    }

    // Verifica os diferentes tipos de aniversário
    checkBirthday() {
        // Verificar a variável ViewBag.Aniversario para aniversário pessoal
        const isUserBirthday = document.getElementById("is-birthday")

        // Verificar a variável ViewBag.AniversarioEmpresa para aniversário da empresa
        const isCompanyAnniversary = document.getElementById("is-company-anniversary")

        // Verificar a variável ViewBag.TempoEmpresa para aniversário de tempo de casa
        const isWorkAnniversary = document.getElementById("is-work-anniversary")

        // Priorizar aniversário de tempo de casa se estiver presente
        if (isWorkAnniversary && isWorkAnniversary.value === "true") {
            // Aguardar um pouco antes de mostrar a animação
            setTimeout(() => this.showWorkAnniversary(), 2000)
        }
        // Depois priorizar aniversário da empresa
        else if (isCompanyAnniversary && isCompanyAnniversary.value === "true") {
            // Aguardar um pouco antes de mostrar a animação
            setTimeout(() => this.showCompanyAnniversary(), 2000)
        }
        // Por último, aniversário pessoal
        else if (isUserBirthday && isUserBirthday.value === "true") {
            // Aguardar um pouco antes de mostrar a animação
            setTimeout(() => this.showPersonalBirthday(), 2000)
        }
    }

    // Cria os elementos necessários para a animação de aniversário pessoal
    createPersonalElements() {
        // Container principal
        this.overlay = document.createElement("div")
        this.overlay.className = "birthday-overlay"
        document.body.appendChild(this.overlay)

        // Canvas para confetes
        this.canvas = document.createElement("canvas")
        this.canvas.id = "birthday-canvas"
        this.overlay.appendChild(this.canvas)

        // Container de conquista
        this.achievement = document.createElement("div")
        this.achievement.className = "birthday-achievement"
        this.overlay.appendChild(this.achievement)

        // Ícone
        const icon = document.createElement("div")
        icon.className = "birthday-icon"
        icon.innerHTML = '<i class="fas fa-birthday-cake"></i>'
        this.achievement.appendChild(icon)

        // Mensagem
        const message = document.createElement("div")
        message.className = "birthday-message"
        message.textContent = "FELIZ ANIVERSÁRIO!"
        this.achievement.appendChild(message)

        // Submensagem
        const submessage = document.createElement("div")
        submessage.className = "birthday-submessage"
        submessage.textContent = "Parabéns pelo seu dia especial! Desejamos muitas felicidades e conquistas!"
        this.achievement.appendChild(submessage)

        // Botão de fechar
        const closeBtn = document.createElement("button")
        closeBtn.className = "birthday-close"
        closeBtn.textContent = "CONTINUAR"
        closeBtn.addEventListener("click", () => this.hideAnimation())
        this.achievement.appendChild(closeBtn)
    }

    // Cria os elementos necessários para a animação de aniversário da empresa
    createCompanyElements() {
        // Container principal
        this.overlay = document.createElement("div")
        this.overlay.className = "birthday-overlay company-anniversary"
        document.body.appendChild(this.overlay)

        // Canvas para confetes
        this.canvas = document.createElement("canvas")
        this.canvas.id = "birthday-canvas"
        this.overlay.appendChild(this.canvas)

        // Container de conquista
        this.achievement = document.createElement("div")
        this.achievement.className = "birthday-achievement"
        this.overlay.appendChild(this.achievement)

        // Ícone
        const icon = document.createElement("div")
        icon.className = "birthday-icon"
        icon.innerHTML = '<i class="fas fa-building"></i>'
        this.achievement.appendChild(icon)

        // Mensagem
        const message = document.createElement("div")
        message.className = "birthday-message"
        message.textContent = "ANIVERSÁRIO DA EMPRESA"
        this.achievement.appendChild(message)

        // Ano de aniversário
        const companyYears = this.getCompanyYears()
        const yearElement = document.createElement("div")
        yearElement.className = "anniversary-year"
        yearElement.textContent = companyYears
        this.achievement.appendChild(yearElement)

        // Submensagem
        const submessage = document.createElement("div")
        submessage.className = "birthday-submessage"
        submessage.textContent = `Celebrando ${companyYears} anos de história, inovação e sucesso! Obrigado por fazer parte desta jornada.`
        this.achievement.appendChild(submessage)

        // Botão de fechar
        const closeBtn = document.createElement("button")
        closeBtn.className = "birthday-close"
        closeBtn.textContent = "CELEBRAR"
        closeBtn.addEventListener("click", () => this.hideAnimation())
        this.achievement.appendChild(closeBtn)

    }

    // Cria os elementos necessários para a animação de aniversário de tempo de empresa
    createWorkAnniversaryElements() {
        // Container principal
        this.overlay = document.createElement("div")
        this.overlay.className = "birthday-overlay work-anniversary"
        document.body.appendChild(this.overlay)

        // Canvas para confetes
        this.canvas = document.createElement("canvas")
        this.canvas.id = "birthday-canvas"
        this.overlay.appendChild(this.canvas)

        // Container de conquista
        this.achievement = document.createElement("div")
        this.achievement.className = "birthday-achievement"
        this.overlay.appendChild(this.achievement)

        // Ícone
        const icon = document.createElement("div")
        icon.className = "birthday-icon"
        icon.innerHTML = '<i class="fas fa-briefcase"></i>'
        this.achievement.appendChild(icon)

        // Obter informações de tempo de empresa
        const workYears = this.getWorkYears()
        const companyName = this.getCompanyName()

        // Mensagem principal
        const message = document.createElement("div")
        message.className = "birthday-message"
        message.textContent = `${workYears} ${workYears === 1 ? "ANO" : "ANOS"}`
        this.achievement.appendChild(message)

        // Submensagem com nome da empresa
        const companyMessage = document.createElement("div")
        companyMessage.className = "company-message"
        companyMessage.textContent = `NO ${companyName}`
        this.achievement.appendChild(companyMessage)

        // Submensagem
        const submessage = document.createElement("div")
        submessage.className = "birthday-submessage"
        submessage.textContent = `Parabéns por ${workYears} ${workYears === 1 ? "ano" : "anos"} de dedicação e conquistas! Sua jornada conosco é motivo de celebração.`
        this.achievement.appendChild(submessage)

        // Botão de fechar
        const closeBtn = document.createElement("button")
        closeBtn.className = "birthday-close"
        closeBtn.textContent = "AGRADECER"
        closeBtn.addEventListener("click", () => this.hideAnimation())
        this.achievement.appendChild(closeBtn)

    }

    // Obtém o número de anos da empresa
    getCompanyYears() {
        // Verificar se há um elemento com o número de anos da empresa
        const companyYearsElement = document.getElementById("company-years")

        if (companyYearsElement && companyYearsElement.value) {
            return companyYearsElement.value
        }

        // Caso contrário, calcular com base no ano de fundação
        const foundingYearElement = document.getElementById("founding-year")

        if (foundingYearElement && foundingYearElement.value) {
            const foundingYear = Number.parseInt(foundingYearElement.value)
            const currentYear = new Date().getFullYear()
            return currentYear - foundingYear
        }

        // Valor padrão se não houver informações
        return 10
    }

    // Obtém o número de anos de trabalho na empresa
    getWorkYears() {
        // Verificar se há um elemento com o número de anos de trabalho
        const workYearsElement = document.getElementById("work-years")

        if (workYearsElement && workYearsElement.value) {
            return Number.parseInt(workYearsElement.value)
        }

        // Valor padrão se não houver informações
        return 1
    }

    // Obtém o nome da empresa
    getCompanyName() {
        // Verificar se há um elemento com o nome da empresa
        const companyNameElement = document.getElementById("company-name")

        if (companyNameElement && companyNameElement.value) {
            return companyNameElement.value.toUpperCase()
        }

        // Valor padrão se não houver informações
        return "GRUPO ROVERI"
    }

    // Exibe a animação de aniversário pessoal
    showPersonalBirthday() {
        if (this.active) return
        this.active = true

        // Criar elementos para aniversário pessoal
        this.createPersonalElements()

        // Mostrar overlay
        setTimeout(() => {
            this.overlay.classList.add("active")

            // Mostrar card e iniciar confetes imediatamente
            this.showAchievement()
        }, 100)
    }

    // Exibe a animação de aniversário da empresa
    showCompanyAnniversary() {
        if (this.active) return
        this.active = true

        // Criar elementos para aniversário da empresa
        this.createCompanyElements()

        // Mostrar overlay
        setTimeout(() => {
            this.overlay.classList.add("active")

            // Mostrar card e iniciar confetes imediatamente
            this.showAchievement(true)
        }, 100)
    }

    // Exibe a animação de aniversário de tempo de empresa
    showWorkAnniversary() {
        if (this.active) return
        this.active = true

        // Criar elementos para aniversário de tempo de empresa
        this.createWorkAnniversaryElements()

        // Mostrar overlay
        setTimeout(() => {
            this.overlay.classList.add("active")

            // Mostrar card e iniciar confetes imediatamente
            this.showAchievement(false, true)
        }, 100)
    }

    // Mostra a conquista de aniversário
    showAchievement(isCompany = false, isWorkAnniversary = false) {
        // Iniciar confetes
        this.startConfetti(isCompany, isWorkAnniversary)

        // Reproduzir som de conquista (opcional)
        if (isWorkAnniversary) {
            this.playSound("work")
        } else if (isCompany) {
            this.playSound("company")
        } else {
            this.playSound("birthday")
        }

        // Mostrar card de conquista
        setTimeout(() => {
            this.achievement.classList.add("show")
        }, 500)
    }

    // Inicia a animação de confetes
    startConfetti(isCompany = false, isWorkAnniversary = false) {
        // Configurar canvas
        this.canvas.width = window.innerWidth
        this.canvas.height = window.innerHeight

        // Iniciar confetti se a biblioteca estiver disponível
        if (window.confetti) {
            const myConfetti = window.confetti.create(this.canvas, { resize: true })

            // Duração dos confetes
            const end = Date.now() + 15 * 1000

            // Cores dos confetes (diferentes para cada tipo de aniversário)
            let colors = ["#ff6000", "#ffffff", "#ffcc00"]

            if (isWorkAnniversary) {
                colors = ["#ff6000", "#282828", "#ffffff", "#ffcc00"]
            } else if (isCompany) {
                colors = ["#ff6000", "#ff8c00", "#ffffff", "#ffcc00"]
            }
            // Função de animação
            ; (function frame() {
                // Mais confetes para aniversário da empresa e tempo de casa
                const particleCount = isCompany || isWorkAnniversary ? 3 : 3

                myConfetti({
                    particleCount: particleCount,
                    angle: 60,
                    spread: 55,
                    origin: { x: 0, y: 0.5 },
                    colors: colors,
                    shapes: ["square", "circle"],
                    scalar: isCompany || isWorkAnniversary ? 1 : 2,
                })

                myConfetti({
                    particleCount: particleCount,
                    angle: 120,
                    spread: 55,
                    origin: { x: 1, y: 0.5 },
                    colors: colors,
                    shapes: ["square", "circle"],
                    scalar: isCompany || isWorkAnniversary ? 1 : 2,
                })

                // Confetes adicionais do topo para aniversário da empresa e tempo de casa
                if (isCompany || isWorkAnniversary) {
                    myConfetti({
                        particleCount: 3,
                        angle: 90,
                        spread: 45,
                        origin: { x: 0.5, y: 0 },
                        colors: colors,
                        shapes: ["square", "circle"],
                        scalar: 3,
                    })
                }

                if (Date.now() < end) {
                    requestAnimationFrame(frame)
                }
            })()
        }
    }

    // Esconde a animação
    hideAnimation() {
        // Remover classe active do overlay
        this.overlay.classList.remove("active")

        // Remover elementos após a transição
        setTimeout(() => {
            this.overlay.remove()
            this.active = false
        }, 500)

        // Verificar o tipo de aniversário
        const isCompanyAnniversary = this.overlay.classList.contains("company-anniversary")
        const isWorkAnniversary = this.overlay.classList.contains("work-anniversary")

        // Mostrar notificação de conquista normal
        if (window.showAchievement) {
            if (isWorkAnniversary) {
                window.showAchievement({
                    title: "TEMPO DE EMPRESA",
                    message: `${this.getWorkYears()} ${this.getWorkYears() === 1 ? "Ano" : "Anos"} no ${this.getCompanyName()}`,
                    description: "Parabéns pela sua dedicação e conquistas!",
                    icon: "briefcase",
                    progress: 100,
                    duration: 6000,
                })
            } else if (isCompanyAnniversary) {
                window.showAchievement({
                    title: "ANIVERSÁRIO DA EMPRESA",
                    message: `${this.getCompanyYears()} Anos de História`,
                    description: "Celebrando nossa jornada de sucesso!",
                    icon: "building",
                    progress: 100,
                    duration: 6000,
                })
            } else {
                window.showAchievement({
                    title: "ANIVERSÁRIO",
                    message: "Feliz Aniversário!",
                    description: "Parabéns pelo seu dia especial!",
                    icon: "birthday-cake",
                    progress: 100,
                    duration: 6000,
                })
            }
        }
    }

    // Reproduz um som
    playSound(type) {
        // Implementar reprodução de som se necessário
        // Exemplo:
        // const sound = new Audio(`/sounds/${type}.mp3`);
        // sound.play();
    }
}

// Inicializar o sistema de animação de aniversário
document.addEventListener("DOMContentLoaded", () => {
    window.BirthdayAnimation = new BirthdayAnimation()
    window.BirthdayAnimation.init()

    // Expor funções para testes
    window.showPersonalBirthday = () => {
        window.BirthdayAnimation.showPersonalBirthday()
    }

    window.showCompanyAnniversary = () => {
        window.BirthdayAnimation.showCompanyAnniversary()
    }

    window.showWorkAnniversary = () => {
        window.BirthdayAnimation.showWorkAnniversary()
    }
})
