"""
Excel 读取工具 - 使用 openpyxl，UTF-8 输出
用法: python excel_read.py --path <文件路径> [--sheet <Sheet名>] [--rows <行数>] [--col <列名>...]
"""
import argparse
import sys
import openpyxl


def main():
    parser = argparse.ArgumentParser(description="读取 Excel 配置表")
    parser.add_argument("--path", required=True, help="Excel 文件路径")
    parser.add_argument("--sheet", default=None, help="Sheet 名称（默认第一个）")
    parser.add_argument("--rows", type=int, default=None, help="最多读取前 N 行数据（不含表头）")
    parser.add_argument("--col", action="append", dest="cols", help="只输出指定列名（可多次使用）")
    args = parser.parse_args()

    try:
        wb = openpyxl.load_workbook(args.path, read_only=True, data_only=True)
    except FileNotFoundError:
        print(f"错误: 文件不存在 -> {args.path}", file=sys.stderr)
        sys.exit(1)

    ws = wb[args.sheet] if args.sheet else wb.active
    if ws is None:
        print(f"错误: 找不到 Sheet '{args.sheet}'", file=sys.stderr)
        wb.close()
        sys.exit(1)

    # 读取表头
    all_rows = list(ws.iter_rows(values_only=True))
    if not all_rows:
        print("（空表）")
        wb.close()
        return

    headers = list(all_rows[0])
    data_rows = all_rows[1:]

    # 过滤列
    if args.cols:
        col_indices = []
        for col_name in args.cols:
            if col_name in headers:
                col_indices.append(headers.index(col_name))
            else:
                print(f"警告: 列 '{col_name}' 不存在，已忽略", file=sys.stderr)
        selected_headers = [headers[i] for i in col_indices]
    else:
        col_indices = list(range(len(headers)))
        selected_headers = headers

    # 限制行数
    if args.rows is not None:
        data_rows = data_rows[:args.rows]

    # 输出表头
    print("\t".join(str(h) if h is not None else "" for h in selected_headers))
    print("-" * 60)

    # 输出数据
    count = 0
    for row in data_rows:
        if all(row[i] is None for i in col_indices):
            continue  # 跳过全空行
        values = [str(row[i]) if row[i] is not None else "" for i in col_indices]
        print("\t".join(values))
        count += 1

    print(f"\n共 {count} 行数据")
    wb.close()


if __name__ == "__main__":
    main()
