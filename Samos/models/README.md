# Modelos ONNX para reconhecimento de placas (ALPR offline)

Esta pasta contém os modelos de IA usados pelo módulo `AlprOnnx.vb` para reconhecer
placas de veículos **100% offline** (sem internet), com alta precisão em fotos reais
(ângulo, pouca luz, perspectiva).

O sistema funciona em **dois estágios**:

1. **Detecção da placa** — localiza a região exata da placa na foto.
2. **Reconhecimento dos caracteres** — lê o texto da placa.

## Arquivos esperados

Coloque os arquivos nesta pasta com **exatamente** estes nomes:

| Arquivo                 | Função                                  | Efeito                                           |
|-------------------------|-----------------------------------------|--------------------------------------------------|
| `plate_detector.onnx`   | Detector da região da placa (YOLO-like) | **Destrava a precisão**: recorta a placa exata   |
| `plate_ocr.onnx`        | Reconhecedor de caracteres (CRNN) — futuro | Reservado para extensão de leitura 100% por IA |

> ### Estado atual sem modelo
> Enquanto **nenhum** `.onnx` estiver presente, o sistema opera em modo
> **fallback seguro**: usa detecção clássica (blackhat + Sobel) para achar a
> placa e o **Tesseract** para ler. Nesse modo, fotos nítidas e frontais são
> lidas bem, mas **fotos noturnas/em ângulo podem não ser reconhecidas** — e,
> nesse caso, o sistema retorna vazio em vez de chutar uma placa errada
> (o operador então digita manualmente no campo Placa).
>
> ### Para precisão ~99% (recomendado)
> Adicione o **`plate_detector.onnx`** nesta pasta. Com ele, a placa é recortada
> com exatidão antes do OCR, o que melhora drasticamente a leitura mesmo em
> fotos difíceis. Após copiar o arquivo, basta reabrir a tela de Ocupação
> (o modelo é carregado automaticamente).

## Onde obter modelos

Modelos ALPR open-source compatíveis com ONNX (verifique a licença de cada um):

- **YOLO de placas** (detecção): treine/exporte um YOLOv5/YOLOv8 para a classe
  "license_plate" e exporte com `export.py ... --include onnx`.
- **Reconhecimento de caracteres**: modelos CRNN/OCR exportados para ONNX.
- Projetos de referência: "Automatic License Plate Recognition" no GitHub
  (ex.: bases YOLO + CRNN para placas Mercosul/BR).

## Formato esperado pela detecção

- **Entrada:** imagem BGR redimensionada para 640x640, normalizada (0–1).
- **Saída:** tensor de detecções no estilo YOLO
  (`[cx, cy, w, h, obj, classe...]` por linha). O parser aplica NMS e
  recorta a melhor caixa com uma margem de segurança.

Se o seu modelo usar outra resolução de entrada, ajuste a constante
`TAMANHO_ENTRADA` em `Samos\Modulos\AlprOnnx.vb`.

## Observação sobre licença

A origem e a licença dos modelos `.onnx` são de responsabilidade de quem os
adiciona aqui. Este projeto apenas executa os modelos fornecidos.
