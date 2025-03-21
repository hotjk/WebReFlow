function ExtractCharacter() {
    let data = [];
    // 遍历文档树
    function traverse(node) {
        if (node.nodeType === Node.TEXT_NODE) {
            processTextNode(node);
        } else if (node.nodeType === Node.ELEMENT_NODE) {
            for (let child of node.childNodes) {
                traverse(child);
            }
        }
    }

    function processTextNode(node) {
        let text = node.nodeValue;
        for (let i = 0; i < text.length; i++) {
            // 跳过空白字符可以根据需要调整
            if (text[i].trim() === '') continue;

            // 使用 Range API 精确测量每个字符
            let range = document.createRange();
            range.setStart(node, i);
            range.setEnd(node, i + 1);
            let rect = range.getBoundingClientRect();

            // 如果字符不可见，可能返回 0
            if (rect.width === 0 && rect.height === 0) continue;

            data.push({
                x: parseInt(rect.x, 10),
                y: parseInt(rect.y, 10),
                w: parseInt(rect.width, 10),
                h: parseInt(rect.height, 10),
                t: text[i]
            });
        }
    }

    traverse(document.body);
    return data;
}
