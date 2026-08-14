"""
Excel 查询/过滤工具 - 使用 openpyxl，UTF-8 输出
用法:
  # 按列名精确匹配
  python excel_find.py --path <文件路径> [--sheet <Sheet名>] --where id=1001
  # 多条件 AND
  python excel_find.py --path <文件路径> --where creature_type=1 --where creature_layer=2
  # 包含匹配（子串）
  python excel_find.py --path <文件路径> --like name=Demon
  # 数值范围
  python excel_find.py --path <文件路径> --gt hp=100 --lt hp=500
  # 只输出指定列
  python excel_find.py --path <文件路径> --where id=1001 --col id --col name --col hp
"""
import argparse
import sys
import openpyxl


def _parse_kv(s):
    if "=" not in s:
        raise argparse.ArgumentTypeError(f"必须为 col=value 格式: {s}")
    k, v = s.split("=", 1)
    return k.strip(), v


def _try_num(s):
    try:
        return int(s)
    except (TypeError, ValueError):
        pass
    try:
        return float(s)
    except (TypeError, ValueError):
        pass
    return None


def main():
    parser = argparse.ArgumentParser(description="查询 Excel 配置表数据")
    parser.add_argument("--path", required=True, help="Excel 文件路径")
    parser.add_argument("--sheet", default=None, help="Sheet 名称（默认第一个）")
    parser.add_argument("--where", action="append", type=_parse_kv, default=[],
                        help="col=value 精确匹配（字符串比较，可多次使用，AND 关系）")
    parser.add_argument("--like", action="append", type=_parse_kv, default=[],
                        help="col=substring 包含匹配（可多次使用）")
    parser.add_argument("--gt", action="append", type=_parse_kv, default=[], help="col=N 大于（数值）")
    parser.add_argument("--lt", action="append", type=_parse_kv, default=[], help="col=N 小于（数值）")
    parser.add_argument("--col", action="append", dest="cols", help="只输出指定列名")
    parser.add_argument("--limit", type=int, default=50, help="最多输出多少行（默认 50）")
    parser.add_argument("--header-rows", type=int, default=3,
                        help="表头行数（默认 3：列名/类型/中文说明；过滤从第 header_rows+1 行开始）")
    args = parser.parse_args()

    try:
        wb = openpyxl.load_workbook(args.path, read_only=True, data_only=True)
    except FileNotFoundError:
        print(f"错误: 文件不存在 -> {args.path}", file=sys.stderr)
        sys.exit(1)

    ws = wb[args.sheet] if args.sheet else wb.active
    all_rows = list(ws.iter_rows(values_only=True))
    if not all_rows:
        print("（空表）")
        wb.close()
        return

    headers = list(all_rows[0])
    name_to_idx = {h: i for i, h in enumerate(headers) if h is not None}

    # 校验列名
    used_cols = (
        [k for k, _ in args.where] + [k for k, _ in args.like]
        + [k for k, _ in args.gt] + [k for k, _ in args.lt]
        + (args.cols or [])
    )
    for c in used_cols:
        if c not in name_to_idx:
            print(f"错误: 列 '{c}' 不存在；可选: {[h for h in headers if h]}", file=sys.stderr)
            wb.close()
            sys.exit(1)

    def match(row):
        for col, expected in args.where:
            actual = row[name_to_idx[col]]
            if actual is None or str(actual) != str(expected):
                return False
        for col, sub in args.like:
            actual = row[name_to_idx[col]]
            if actual is None or sub not in str(actual):
                return False
        for col, threshold in args.gt:
            actual = row[name_to_idx[col]]
            t = _try_num(threshold)
            a = actual if isinstance(actual, (int, float)) else _try_num(actual)
            if a is None or t is None or not (a > t):
                return False
        for col, threshold in args.lt:
            actual = row[name_to_idx[col]]
            t = _try_num(threshold)
            a = actual if isinstance(actual, (int, float)) else _try_num(actual)
            if a is None or t is None or not (a < t):
                return False
        return True

    if args.cols:
        out_indices = [name_to_idx[c] for c in args.cols]
        out_headers = list(args.cols)
    else:
        out_indices = list(range(len(headers)))
        out_headers = [str(h) if h is not None else "" for h in headers]

    print("\t".join(out_headers))
    print("-" * 60)

    count = 0
    matched = 0
    for row in all_rows[args.header_rows:]:
        if all(v is None for v in row):
            continue
        if not match(row):
            continue
        matched += 1
        if count >= args.limit:
            continue
        print("\t".join(str(row[i]) if row[i] is not None else "" for i in out_indices))
        count += 1

    print(f"\n匹配 {matched} 行，已显示 {count} 行（limit={args.limit}）")
    wb.close()


if __name__ == "__main__":
    main()
