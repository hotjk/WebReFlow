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
        var row = null;
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

            var char = {
                x: rect.x,
                y: rect.y,
                t: text[i]
            };

            if (row === null) {
                row = { ...char };
            } else if (Math.abs(row.y - char.y) < 1) {
                row.t += char.t;
                //row.x = Math.min(row.x, char.x);
            } else
            {
                data.push(
                    {
                        x: parseInt(row.x, 10),
                        y: parseInt(row.y, 10),
                        t: row.t
                    }
                );
                row = { ...char };
            }
        }
        if (row !== null) {
            data.push(
                {
                    x: parseInt(row.x, 10),
                    y: parseInt(row.y, 10),
                    t: row.t
                }
            );
        }
    }

    traverse(document.body);
    return data;
}
