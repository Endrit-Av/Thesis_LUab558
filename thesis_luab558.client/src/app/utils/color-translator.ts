export function translateColor(color: string): string {
  switch (color.toLowerCase()) {
    case 'blau': return 'blue';
    case 'rot': return 'red';
    case 'grün': return 'green';
    case 'gelb': return 'yellow';
    case 'orange': return 'orange';
    case 'lila': return 'purple';
    case 'rosa': return 'pink';
    case 'braun': return 'brown';
    case 'grau': return 'gray';
    case 'schwarz': return 'black';
    case 'weiß': return 'white';
    case 'weiss': return 'white';
    case 'silber': return 'silver';
    default: return 'notInStock';
  }
}
