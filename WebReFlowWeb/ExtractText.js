function ExtractText() {
    let data = [];
    // 遍历文档树
    function traverse(node) {
        if (node.nodeType === Node.TEXT_NODE) {
            processDivNode(node);
        } else if (node.nodeType === Node.ELEMENT_NODE) {
            for (let child of node.childNodes) {
                traverse(child);
            }
        }
    }

    function processDivNode(node) {
        let text = node.nodeValue;
        const rect = text.getBoundingClientRect();
        
        data.push({
            x: parseInt(rect.x, 10),
            y: parseInt(rect.y, 10),
            w: parseInt(rect.width, 10),
            h: parseInt(rect.height, 10),
            t: text
        });
    }

    function processTextNode(node) {
        let text = node.nodeValue;
        const range = document.createRange();
        range.selectNodeContents(node);

        const span = document.createElement('span');
        span.appendChild(range.extractContents());

        node.parentNode.insertBefore(span, node);
        const rect = span.getBoundingClientRect();

        span.parentNode.replaceChild(range.extractContents(), span);
        range.detach();
        data.push({
            x: parseInt(rect.x, 10),
            y: parseInt(rect.y, 10),
            w: parseInt(rect.width, 10),
            h: parseInt(rect.height, 10),
            t: text
        });
    }

    traverse(document.body);
    return data;
}
