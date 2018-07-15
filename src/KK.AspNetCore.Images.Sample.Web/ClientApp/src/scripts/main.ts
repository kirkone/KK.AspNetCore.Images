export class Main {
    public CheckSnap() {
        if ('scrollSnapType' in document.documentElement.style ||
            'webkitScrollSnapType' in document.documentElement.style ||
            'msScrollSnapType' in document.documentElement.style) {
            console.log('Browser does suport scroll snap.');
        }
        else {
            this.noSnapFallback();
        }
    }

    private noSnapFallback() {
        console.log('Browser does NOT suport scroll snap.');

        const element = document.getElementById('content');
        if (element) {
            element.classList.add("no-snap-fallback");
        }
    }
}