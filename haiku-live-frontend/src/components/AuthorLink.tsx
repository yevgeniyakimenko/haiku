import { Author } from '../../types'

interface AuthorLinkProps {
  author: Author
  selected: boolean
  onClick: (author: Author) => void
}

export default function AuthorLink({ author, selected, onClick }: AuthorLinkProps) {
  const selAuth: string = selected ? 'font-extrabold text-orange-600 dark:text-orange-500 hover:text-orange-600 hover:dark:text-orange-500' : ''
  const handleClick: () => void = () => {
    onClick(author)
  }

  return (
    <li className={`mb-3 cursor-pointer break-all hover:text-orange-500 hover:font-bold ${selAuth}`} onClick={handleClick}>
      {selected ? '🍂 ' : ''}
      {author.name}
    </li>
  )
}
