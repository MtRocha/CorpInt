/**
 * Dynamic Script Loader
 * Loads scripts on demand to improve initial page load performance
 */
const ScriptLoader = {
    loadedScripts: {},

    // Load a script dynamically
    load: function (url, callback) {
        if (this.loadedScripts[url]) {
            if (callback) callback();
            return Promise.resolve();
        }

        return new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = url;
            script.async = true;

            script.onload = () => {
                this.loadedScripts[url] = true;
                if (callback) callback();
                resolve();
            };

            script.onerror = () => {
                reject(new Error(`Failed to load script: ${url}`));
            };

            document.body.appendChild(script);
        });
    },

    // Load multiple scripts in sequence
    loadSequential: function (urls, finalCallback) {
        const loadNext = (index) => {
            if (index >= urls.length) {
                if (finalCallback) finalCallback();
                return Promise.resolve();
            }

            return this.load(urls[index]).then(() => loadNext(index + 1));
        };

        return loadNext(0);
    },

    // Load feature-specific scripts
    loadFeature: function (feature) {
        switch (feature) {
            case 'feed':
                return this.loadSequential([
                    '/js/Reacao.js',
                    '/js/Comentario.js',
                    '/js/Publicacao.js',
                    '/js/FeedPaginado.js'
                ]);
            case 'chat':
                return this.loadSequential([
                    'https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/7.0.5/signalr.min.js',
                    '/js/Chat.js'
                ]);
            case 'editor':
                return this.load('/lib/tinymce/tinymce.min.js', function () {
                    // Initialize TinyMCE after loading
                    if (typeof tinymce !== 'undefined') {
                        tinymce.init({
                            selector: '.rich-text-editor',
                            // Your TinyMCE configuration
                        });
                    } else {
                        console.error('TinyMCE failed to load.');
                    }
                });
            case 'quizz':
                return this.loadSequential([
                    'https://cdn.jsdelivr.net/npm/canvas-confetti@1.5.1',
                    '/js/Quizz.js'
                ]);
            default:
                return Promise.resolve();
        }
    }
};

// Make it globally available
window.ScriptLoader = ScriptLoader;